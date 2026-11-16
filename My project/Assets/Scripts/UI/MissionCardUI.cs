using UnityEngine;
using UnityEngine.UI;

public class MissionCardUI : MonoBehaviour
{
    public MissionData missionData;

    [Header("UI Elements")]
    public Image cardImage;
    public Text titleText;
    public Button cardButton;

    private MissionMenuManager menuManager;

    void Start()
    {
        menuManager = FindObjectOfType<MissionMenuManager>();

        if (missionData != null)
        {
            if (cardImage != null && missionData.missionImage != null)
            {
                cardImage.sprite = missionData.missionImage;
            }

            if (titleText != null)
                titleText.text = missionData.missionTitle;

            cardButton.onClick.AddListener(OnCardClicked);
        }
    }

    void OnCardClicked()
    {
        if (menuManager != null)
        {
            // HAPUS PARAMETER (missionData) DI SINI:
            menuManager.OpenDetail();
        }
    }
}