using UnityEngine;
using UnityEngine.UI;

public class MissionCardUI : MonoBehaviour
{
    public MissionData missionData;

    [Header("UI Elements")]
    public Image cardImage;   // Untuk Gambar
    public Text titleText;    // Untuk Judul
    public Button cardButton; // Tombolnya sendiri

    private MissionMenuManager menuManager;

    void Start()
    {
        menuManager = FindObjectOfType<MissionMenuManager>();

        if (missionData != null)
        {
            // 1. Set Gambar dari MissionData
            if (cardImage != null && missionData.missionImage != null)
            {
                cardImage.sprite = missionData.missionImage;
            }

            // 2. Set Judul
            if (titleText != null)
                titleText.text = missionData.missionTitle;

            // 3. Setup tombol klik
            cardButton.onClick.AddListener(OnCardClicked);
        }
    }

    void OnCardClicked()
    {
        if (menuManager != null && missionData != null)
        {
            menuManager.OpenDetail(missionData);
        }
    }
}