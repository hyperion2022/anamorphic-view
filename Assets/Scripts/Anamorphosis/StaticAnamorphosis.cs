using UnityEngine;

public class StaticAnamorphosis : MonoBehaviour
{
    [SerializeField] float nFactor = 1f;
    [SerializeField] float fFactor = 4f;
    [SerializeField] Camera targetCamera;

    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        calculateAnamorphosis(mesh, nFactor, fFactor, targetCamera);
    }

    private void calculateAnamorphosis(Mesh mesh, float nFactor, float fFactor, Camera camera)
    {
        // (1) Define a box around the vertices b_i.
        Vector3[] b = mesh.vertices;
        this.gameObject.AddComponent<OrientedBoundingBox>();
        OrientedBoundingBox obb = GetComponent<OrientedBoundingBox>();
        obb.OrientedBoundingBoxConstructor(camera, this.gameObject);

        // (2) Choose one face of this box.
        float d = obb.CalculateDistanceBetweenClosestAndOpposingFace(camera);

        // (3) Specify a field-of-view angle.
        float n = obb.CalculateNearDistance(camera);

        // (4) Assign the far distance of the box.
        float f = n + d;

        // (5) Input a near and far distance.
        float n_p = nFactor * n;
        float f_p = fFactor * f;

        float r = (f - n) / (f_p - n_p);
        Vector3[] p = new Vector3[b.Length];
        for (int i = 0; i < b.Length; ++i)
        {
            // (6) Transform the vertices to eye coordinates.
            p[i] = camera.WorldToViewportPoint(transform.TransformPoint(b[i]));

            // (7) Apply (7) and then (8) to each vertex of the model.
            p[i].z = (r * n_p * f_p) / (r * n_p + f - b[i].z);
            p[i].x = b[i].x * p[i].z / n;
            p[i].y = b[i].y * p[i].z / n;

            // (8) (Optional) Return the vertices to their original coordinate frame.
            // b[i] = camera.ViewportToWorldPoint(p[i]);
        }

        mesh.vertices = p;
        mesh.RecalculateBounds();
    }
}