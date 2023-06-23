// Source: https://www.youtube.com/watch?v=sVCaAkocdj4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 cameraPosition = _mainCamera.transform.position;
        transform.LookAt(transform.position + (transform.position - cameraPosition), _mainCamera.transform.up);
    }
}

