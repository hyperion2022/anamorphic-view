// Source: https://www.youtube.com/watch?v=THmW4YolDok

using UnityEngine;
using UnityEngine.UI;

public class Host : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] Image textPanel;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        textPanel.gameObject.SetActive(true);
        Debug.Log("Talking to Host!");
        return true;
    }
}
