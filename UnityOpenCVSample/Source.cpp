#include "opencv2/objdetect.hpp"
#include "opencv2/highgui.hpp"
#include "opencv2/imgproc.hpp"
#include <opencv2/dnn.hpp>
#include <iostream>
#include <stdio.h>

using namespace std;
using namespace cv;
using namespace dnn;

// Declare structure to be used to pass data from C++ to Mono.
struct Circle
{
	Circle(int x, int y, int radius, uchar depth) : X(x), Y(y), Radius(radius), Depth(depth) {}
	int X, Y, Radius, Depth;
};

CascadeClassifier _faceCascade;
String _windowName = "Unity OpenCV Interop Sample";
VideoCapture _capture;
int _scale = 1;

extern "C" int __declspec(dllexport) __stdcall  Init(int& outCameraWidth, int& outCameraHeight)
{
	// Load LBP face cascade.
	if (!_faceCascade.load("lbpcascade_frontalface.xml"))
		return -1;

	// Open the stream.
	_capture.open(0);
	if (!_capture.isOpened())
		return -2;

	outCameraWidth = _capture.get(CAP_PROP_FRAME_WIDTH);
	outCameraHeight = _capture.get(CAP_PROP_FRAME_HEIGHT);

	return 0;
}

extern "C" void __declspec(dllexport) __stdcall  Close()
{
	_capture.release();
}

extern "C" void __declspec(dllexport) __stdcall SetScale(int scale)
{
	_scale = scale;
}

extern "C" void __declspec(dllexport) __stdcall Detect(Circle * outFaces, int maxOutFacesCount, int& outDetectedFacesCount)
{
	Mat frame;
	_capture >> frame;
	if (frame.empty())
		return;

	// Load the DenseDepth model
	string model_path = "densedepth.onnx";
	Net model = readNetFromONNX(model_path);

	// Load and preprocess the input image
	Mat input_blob = blobFromImage(frame, 1 / 255.0, Size(640, 480), Scalar(0, 0, 0), true, false);

	// Set the input blob to the network
	model.setInput(input_blob);

	// Forward pass through the network
	Mat output_blob = model.forward();

	// Convert the output blob to a depth map
	Mat depth_map(output_blob.size[2], output_blob.size[3], CV_32F, output_blob.ptr<float>());

	// Normalize the depth map for visualization
	normalize(depth_map, depth_map, 0, 255, NORM_MINMAX, CV_8U);

	vector<Rect> faces;
	// Convert the frame to grayscale for cascade detection.
	Mat grayscaleFrame;
	cvtColor(frame, grayscaleFrame, COLOR_BGR2GRAY);
	Mat resizedGray;
	// Scale down for better performance.
	resize(grayscaleFrame, resizedGray, Size(frame.cols / _scale, frame.rows / _scale));
	equalizeHist(resizedGray, resizedGray);

	// Detect faces.
	_faceCascade.detectMultiScale(resizedGray, faces);

	// Draw faces.
	for (size_t i = 0; i < faces.size(); i++)
	{
		Point center(_scale * (faces[i].x + faces[i].width / 2), _scale * (faces[i].y + faces[i].height / 2));
		ellipse(frame, center, Size(_scale * faces[i].width / 2, _scale * faces[i].height / 2), 0, 0, 360, Scalar(0, 0, 255), 4, 8, 0);

		// Send to application.
		outFaces[i] = Circle(faces[i].x, faces[i].y, faces[i].width / 2, depth_map.at<uchar>(faces[i].x, faces[i].y));
		outDetectedFacesCount++;

		if (outDetectedFacesCount == maxOutFacesCount)
			break;
	}

	// Display debug output.
	imshow(_windowName, frame);
	// imshow("Depth Map", depth_map);
}