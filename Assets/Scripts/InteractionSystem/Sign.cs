// Source: https://www.youtube.com/watch?v=THmW4YolDok

using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] Image textPanel;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        textPanel.gameObject.SetActive(true);

        Debug.Log("Reading sign!");
        return true;
    }
}
