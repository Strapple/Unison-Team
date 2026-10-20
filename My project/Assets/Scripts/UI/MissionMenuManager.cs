using UnityEngine;
using UnityEngine.UI; // Wajib ada untuk Image, Button, dan Text biasa

public class MissionMenuManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject missionListPanel;
    public GameObject missionDetailPanel;
    public GameObject environment3D;
    public GameObject endDayButton;

    [Header("Detail UI Elements (Standard Text)")]
    public Image detailImage;
    public Text detailTitle;        // <-- Diubah ke Text biasa
    public Text detailDescription;  // <-- Diubah ke Text biasa
    public Text detailRewardText;   // <-- Diubah ke Text biasa
    public Button startButton;

    private MissionData selectedMission;

    void Start()
    {
        // Awal game: Tampilkan List, matikan Detail & 3D Map
        missionListPanel.SetActive(true);
        missionDetailPanel.SetActive(false);
        environment3D.SetActive(false);
    }

    // Dipanggil saat Card Misi diklik
    public void OpenDetail(MissionData mission)
    {
        selectedMission = mission;
        missionListPanel.SetActive(false);
        missionDetailPanel.SetActive(true);

        // Isi Data UI
        if (mission.missionImage != null)
            detailImage.sprite = mission.missionImage;

        detailTitle.text = mission.missionTitle;
        detailDescription.text = mission.description;

        int totalCost = (mission.reqObat * mission.hargaObat) + (mission.reqBeras * mission.hargaBeras);
        detailRewardText.text = $"Reward: +{mission.rewardTrust}% Trust\n(Uang kembali: ${totalCost})";
    }

    public void BackToList()
    {
        missionDetailPanel.SetActive(false);
        missionListPanel.SetActive(true);
    }

    public void StartMission()
    {
        missionListPanel.SetActive(false);
        missionDetailPanel.SetActive(false);

        if (environment3D != null) environment3D.SetActive(true);

        // Munculkan tombol End Day
        if (endDayButton != null) endDayButton.SetActive(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartMission(selectedMission);
        }
    }
}