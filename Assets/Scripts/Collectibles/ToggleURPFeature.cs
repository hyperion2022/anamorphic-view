using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleURPFeature : MonoBehaviour
{
    public MouseButton toggleButton = MouseButton.Right;
    bool isActive = false;
    [SerializeField] UniversalRendererData feature;
    [SerializeField] List<GameObject> despawnList;
    [SerializeField] List<GameObject> spawnList;
    [SerializeField] GameObject anaglyphGlasses;

    private void Start()
    {
        Debug.Log("Press [Mouse Right] to activate/deactivate the anaglyph effect.");
        feature.rendererFeatures[0].SetActive(isActive);

        foreach (GameObject o in spawnList) o.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)toggleButton))
        {
            isActive = !isActive;

            anaglyphGlasses.SetActive(isActive);
            foreach (GameObject o in despawnList) o.SetActive(!isActive);
            foreach (GameObject o in spawnList) o.SetActive(isActive);

            feature.rendererFeatures[0].SetActive(isActive);
        }
    }
}
