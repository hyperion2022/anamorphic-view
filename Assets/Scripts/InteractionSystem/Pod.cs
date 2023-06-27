// Source: https://www.youtube.com/watch?v=THmW4YolDok

using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] Image textPanel;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        textPanel.gameObject.SetActive(true);
        Debug.Log("Opening console!");
        return true;
    }
}
