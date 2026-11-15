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
        // 1. Matikan Menu
        canvasMenu.SetActive(false);

        // 2. Simpan Misi ke GameManager agar bisa dibaca oleh Toko dan Peta
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartMission(selectedMission);
        }

        // 3. JANGAN NYALAKAN PETA DULU! Matikan Peta dan HUD Gameplay.
        if (environment3D != null) environment3D.SetActive(false);
        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (endDayButton != null) endDayButton.SetActive(false);

        // Roll Event sebelum masuk ke Toko
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RollRandomEvent();
        }

        // 4. PINDAH KE TOKO (MiniGame)
        if (MiniGameManager.Instance != null)
        {
            MiniGameManager.Instance.StartMiniGame();
        }
        else
        {
            Debug.LogError("MiniGameManager tidak ditemukan di Scene! Pastikan script Manager_MiniGame aktif.");
        }
    }
}