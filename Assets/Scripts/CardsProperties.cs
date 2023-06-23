using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CardsProperties : MonoBehaviour
{
    [SerializeField] List<GameObject> cardParents;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var cardParent in cardParents)
        {
            // Loop through each child of the parent object
            for (int i = 0; i < cardParent.transform.childCount; i++)
            {
                // Get the child Transform at index i
                Transform childTransform = cardParent.transform.GetChild(i);

                // Get the child game object from the child Transform
                GameObject childObject = childTransform.gameObject;

                // Do something with the child game object
                childObject.AddComponent<CardRotation>();

                BoxCollider box = childObject.AddComponent<BoxCollider>();
                box.size = new Vector3(0.5f, 0.07f, 0.75f);

                if (cardParent.name == "Diamonds") childObject.tag = "Diamond";
                else if (cardParent.name == "Hearts") childObject.tag = "Heart";
                else if (cardParent.name == "Spades") childObject.tag = "Spade";
                else childObject.tag = "Club";

                /*GameObject lightObject = new GameObject();
                lightObject.transform.parent = childObject.transform;
                Light spotlight = lightObject.AddComponent<Light>();
                spotlight.type = UnityEngine.LightType.Spot;
                spotlight.spotAngle = 45f;
                spotlight.range = 100f;*/
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
