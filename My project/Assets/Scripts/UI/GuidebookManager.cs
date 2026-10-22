using UnityEngine;
using UnityEngine.UI;

public class GuidebookManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject guidebookPanel;

    [Header("UI Content")]
    public Text txtTitle;
    public Text txtSolution;

    [Header("Halaman Guidebook (Isi Manual di Inspector)")]
    public string[] pageTitles;
    [TextArea] public string[] pageSolutions;

    private int currentPageIndex = 0;

    void Start()
    {
        if (guidebookPanel != null) guidebookPanel.SetActive(false);
    }

    public void OpenGuidebook()
    {
        currentPageIndex = 0;
        guidebookPanel.SetActive(true);
        UpdatePageUI();
    }

    public void CloseGuidebook()
    {
        guidebookPanel.SetActive(false);
    }

    public void NextPage()
    {
        if (currentPageIndex < pageTitles.Length - 1)
        {
            currentPageIndex++;
            UpdatePageUI();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageUI();
        }
    }

    void UpdatePageUI()
    {
        if (pageTitles.Length > 0 && pageSolutions.Length > 0)
        {
            txtTitle.text = pageTitles[currentPageIndex];
            txtSolution.text = pageSolutions[currentPageIndex];
        }
    }
}