using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndTextScroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pageNumberText;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] Image nextButton;
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] string[] pageText;

    private int currentPage;

    private void updatePage(int currentPage)
    {
        pageNumberText.text = "(" + (currentPage + 1) + " / " + pageText.Length + ")";
        bodyText.text = pageText[currentPage];
        if (currentPage == pageText.Length - 1)
        {
            instructionText.text = "[F] Close";
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            instructionText.text = "[F] Next Page";
        }
    }

    private void Start()
    {
        pageNumberText = GameObject.Find("Page Number").GetComponent<TextMeshProUGUI>();
        bodyText = GameObject.Find("Body").GetComponent<TextMeshProUGUI>();
        nextButton = GameObject.Find("Next Button").GetComponent<Image>();
        instructionText = GameObject.Find("Instruction").GetComponent<TextMeshProUGUI>();

        // First slide
        currentPage = 0;
        updatePage(currentPage);
    }

    private void OnEnable()
    {
        currentPage = 0;
        if (pageNumberText != null) { updatePage(currentPage); }
        
    }

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            currentPage++;
            if (currentPage < pageText.Length)
            {
                updatePage(currentPage);
            }
            else
            {
                SceneManager.LoadScene("Ending");
                this.gameObject.SetActive(false);
            }
        }
        
    }
}
