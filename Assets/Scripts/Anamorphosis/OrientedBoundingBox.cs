// OrientedBoundingBox class based on code by OpenAI's ChatGPT assistant.
using UnityEngine;

public class OrientedBoundingBox : MonoBehaviour
{
    private Vector3 center;
    private Vector3 size;
    private Quaternion rotation;

    public void OrientedBoundingBoxConstructor(Camera camera, GameObject targetObject)
    {
        Initialize(camera, targetObject);
    }

    private void Initialize(Camera camera, GameObject targetObject)
    {
        // Calculate center of the OBB
        Vector3 cameraToObject = targetObject.transform.position - camera.transform.position;
        float distance = Vector3.Dot(cameraToObject, camera.transform.forward);
        center = camera.transform.position + camera.transform.forward * distance;

        // Calculate size of the OBB
        Renderer objectRenderer = targetObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            Bounds objectBounds = objectRenderer.bounds;
            size = objectBounds.size;
        }
        else
        {
            size = Vector3.zero;
            Debug.LogWarning("Target object does not have a renderer. Setting OBB size to zero.");
        }

        // Calculate rotation of the OBB
        Vector3 lookDir = cameraToObject.normalized;
        Vector3 upDir = Vector3.up;
        rotation = Quaternion.LookRotation(-lookDir, upDir);
    }

    public Vector3[] GetCorners()
    {
        Vector3[] corners = new Vector3[6];

        Vector3 extents = size * 0.5f;
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

        corners[0] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, -extents.z));
        corners[1] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z));
        corners[2] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z));
        corners[3] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z));
        corners[4] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z));
        corners[5] = center + rotationMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z));

        return corners;
    }

    public int GetClosestFaceIndex(Camera camera)
    {
        Vector3[] corners = GetCorners();
        Vector3 cameraPosition = camera.transform.position;

        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < 6; i++)
        {
            int a = (i + 1) % 6;

            Vector3 faceCenter = (corners[i] + corners[a]) / 2f;
            float distance = Vector3.Distance(cameraPosition, faceCenter);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public float CalculateDistanceBetweenClosestAndOpposingFace(Camera camera)
    {
        Vector3[] corners = GetCorners();
        Vector3 cameraPosition = camera.transform.position;

        int closestIndex = GetClosestFaceIndex(camera);
        int opposingIndex = (closestIndex + 3) % 6; // Calculate the index of the opposing face

        Vector3 closestFaceCenter = GetFaceCenter(corners, closestIndex);
        Vector3 opposingFaceCenter = GetFaceCenter(corners, opposingIndex);

        return Vector3.Distance(closestFaceCenter, opposingFaceCenter);
    }

    private Vector3 GetFaceCenter(Vector3[] corners, int faceIndex)
    {
        int a = (faceIndex + 1) % 6;
        int b = (faceIndex + 3) % 6;

        return (corners[faceIndex] + corners[a] + corners[b]) / 3f;
    }

    public float CalculateNearDistance(Camera camera)
    {
        float fieldOfView = Mathf.Clamp(camera.fieldOfView, 0, 180);
        Vector3[] corners = GetCorners();
        Vector3 cameraPosition = camera.transform.position;

        int closestIndex = GetClosestFaceIndex(Camera.main);
        int opposingIndex = (closestIndex + 3) % 6;

        Vector3 closestFaceCenter = GetFaceCenter(corners, closestIndex);
        Vector3 opposingFaceCenter = GetFaceCenter(corners, opposingIndex);

        float distance = Vector3.Distance(closestFaceCenter, opposingFaceCenter);
        float n = (0.5f * distance) / Mathf.Tan(0.5f * Mathf.Deg2Rad * fieldOfView);

        return n;
    }
}
