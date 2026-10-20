using UnityEngine;
using UnityEngine.UI;

public class MissionCardUI : MonoBehaviour
{
    public MissionData missionData; // Tempat naruh file ScriptableObject
    public Text titleText;          // Tempat naruh teks judul
    public Button cardButton;       // Tombolnya sendiri

    private MissionMenuManager menuManager;

    void Start()
    {
        // Cari MenuManager di scene
        menuManager = FindObjectOfType<MissionMenuManager>();

        if (missionData != null)
        {
            // INI KUNCI UTAMANYA: Ubah teks "Title" menjadi judul dari SO
            titleText.text = missionData.missionTitle;

            // Tambahkan fungsi klik
            cardButton.onClick.AddListener(OnCardClicked);
        }
        else
        {
            Debug.LogError("MissionData belum dimasukkan ke Card!");
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