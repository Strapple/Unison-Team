using UnityEngine;
using UnityEngine.UI;

public class MissionMenuManager : MonoBehaviour
{
    [Header("UI Menu References")]
    public GameObject missionListPanel;
    public GameObject missionDetailPanel;
    public GameObject canvasMenu; // Canvas_Menu itu sendiri

    [Header("Gameplay References")]
    public GameObject environment3D; // Peta 3D
    public GameObject canvasGameplay; // HUD (Uang, Truk, dll)
    public GameObject endDayButton; // Tombol End Day (jika belum diurus HUDManager)

    [Header("Detail UI Elements")]
    public Image detailImage;
    public Text detailTitle;
    public Text detailDescription;
    public Text detailRewardText;

    private MissionData selectedMission;

    void Start()
    {
        // Pastikan saat mulai, Menu nyala, Gameplay mati
        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (environment3D != null) environment3D.SetActive(false);
    }

    public void OpenDetail(MissionData mission)
    {
        selectedMission = mission;
        missionListPanel.SetActive(false);
        missionDetailPanel.SetActive(true);

        if (mission.missionImage != null) detailImage.sprite = mission.missionImage;
        detailTitle.text = mission.missionTitle;
        detailDescription.text = mission.description;

        // Sesuaikan dengan sistem puzzle baru
        detailRewardText.text = $"Reward: +{mission.rewardTrust}% Trust\n(Estimasi Profit: ${mission.profitUang})";
    }

    public void BackToList()
    {
        missionDetailPanel.SetActive(false);
        missionListPanel.SetActive(true);
    }

    public void StartMission()
    {
        // 1. Matikan Menu
        canvasMenu.SetActive(false);

        // 2. Nyalakan Peta & Gameplay UI
        if (environment3D != null) environment3D.SetActive(true);
        if (canvasGameplay != null) canvasGameplay.SetActive(true);

        // 3. Mulai Misi di GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartMission(selectedMission);
        }

        // 4. Munculkan tombol End Day (jika diurus di sini)
        if (endDayButton != null) endDayButton.SetActive(true);
    }
}