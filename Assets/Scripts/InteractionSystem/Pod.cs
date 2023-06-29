// Source: https://www.youtube.com/watch?v=THmW4YolDok

using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] Image tutorialPanel;
    [SerializeField] Image endPanel;
    [SerializeField] GameObject player;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        ThirdPersonController tpc = player.GetComponent<ThirdPersonController>();
        if (!tpc.PlayerHasAllSymbols())
        {
            tutorialPanel.gameObject.SetActive(true);
        }
        else
        {
            endPanel.gameObject.SetActive(true);
        }
        
        Debug.Log("Opening console!");
        return true;
    }
}
