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
        // 1. Paksa nyalakan Canvas & List
        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (missionListPanel != null) missionListPanel.SetActive(true);

        // 2. Paksa matikan Detail, Gameplay, dan Peta
        if (missionDetailPanel != null) missionDetailPanel.SetActive(false);
        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (environment3D != null) environment3D.SetActive(false);

        // Opsional: Cek apakah berhasil (muncul di Console)
        Debug.Log("Start Menu dijalankan. List Panel status: " + missionListPanel.activeSelf);
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
        canvasMenu.SetActive(false);

        // 1. Simpan misi DULU ke GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.StartMission(selectedMission);

        // 2. Nyalakan Peta & UI
        if (environment3D != null) environment3D.SetActive(true);
        if (canvasGameplay != null) canvasGameplay.SetActive(true);
        if (endDayButton != null) endDayButton.SetActive(true);

        // 3. TAMBAHKAN INI: Paksa HUD nyalakan titik merah
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null) hud.UpdateReportNotif();
    }
}