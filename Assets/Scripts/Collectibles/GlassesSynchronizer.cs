// GlassesSynchronizer class based on code by OpenAI's ChatGPT assistant.

using UnityEngine;

public class GlassesSynchronizer : MonoBehaviour
{
    public Transform headTransform;  // Reference to the head GameObject in the character hierarchy
    public Transform glassesTransform;  // Reference to the glasses GameObject

    private Vector3 initialOffset;  // Initial position offset between head and glasses

    private void Start()
    {
        initialOffset = glassesTransform.position - headTransform.position;
    }

    private void LateUpdate()
    {
        // Synchronize the position and rotation of the glasses with the head
        glassesTransform.position = headTransform.position + initialOffset;
        glassesTransform.rotation = headTransform.rotation;
    }
}
