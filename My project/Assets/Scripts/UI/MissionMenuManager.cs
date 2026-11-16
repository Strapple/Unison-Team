using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionMenuManager : MonoBehaviour
{
    [Header("UI Menu References")]
    public GameObject missionListPanel;
    public GameObject missionDetailPanel;
    public GameObject canvasMenu;

    [Header("Gameplay References")]
    public GameObject environment3D;
    public GameObject canvasGameplay;
    public GameObject endDayButton;

    [Header("Detail UI Elements")]
    public Image detailImage;
    public Text detailTitle;
    public Text detailDescription;
    public Text detailRewardText;

    [Header("Navigation UI (Tombol Panah)")]
    public Button btnPrevMission;
    public Button btnNextMission;

    [Header("Data Misi (Tarik ScriptableObject ke sini)")]
    public MissionData missionData1;
    public MissionData missionData2;

    private List<MissionData> todaysMissions = new List<MissionData>();
    private int currentDetailIndex = 0;

    void Start()
    {
        // Panggil fungsi reset saat game pertama kali dimulai
        ResetToMenu();
    }

    // Fungsi ini menggantikan Start() agar bisa dipanggil dari script Summary
    public void ResetToMenu()
    {
        Debug.Log(">>> MERESET UI KE MENU UTAMA <<<");

        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (missionListPanel != null) missionListPanel.SetActive(true);
        if (missionDetailPanel != null) missionDetailPanel.SetActive(false);
        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (environment3D != null) environment3D.SetActive(false);
        if (endDayButton != null) endDayButton.gameObject.SetActive(false);

        // Sembunyikan Summary
        SummaryManager summary = FindObjectOfType<SummaryManager>();
        if (summary != null && summary.summaryPanel != null)
        {
            summary.summaryPanel.SetActive(false);
        }

        // Susun Misi Berdasarkan Hari
        todaysMissions.Clear();
        if (GameManager.Instance.currentDay == 1)
        {
            if (missionData1 != null) todaysMissions.Add(missionData1);
        }
        else
        {
            if (missionData1 != null) todaysMissions.Add(missionData1);
            if (missionData2 != null) todaysMissions.Add(missionData2);
            else Debug.LogError("MISSION DATA 2 KOSONG DI INSPECTOR!");
        }

        Debug.Log("HARI INI ADA " + todaysMissions.Count + " MISI.");
    }

    public void OpenDetail()
    {
        currentDetailIndex = 0;
        missionListPanel.SetActive(false);
        missionDetailPanel.SetActive(true);
        UpdateDetailUI();
    }

    public void NextMissionDetail()
    {
        if (currentDetailIndex < todaysMissions.Count - 1)
        {
            currentDetailIndex++;
            UpdateDetailUI();
        }
    }

    public void PrevMissionDetail()
    {
        if (currentDetailIndex > 0)
        {
            currentDetailIndex--;
            UpdateDetailUI();
        }
    }

    void UpdateDetailUI()
    {
        MissionData mission = todaysMissions[currentDetailIndex];

        if (mission.missionImage != null) detailImage.sprite = mission.missionImage;
        detailTitle.text = mission.missionTitle;
        detailDescription.text = mission.description;

        detailRewardText.text = $"Reward: +{mission.rewardTrust}% Trust\n(Estimasi Profit: ${mission.profitUang})\n\n" +
                                $"<color=yellow>Rekomendasi Kebutuhan:\nKirim {mission.correctAmount} {mission.correctItemName}</color>";

        if (btnPrevMission != null) btnPrevMission.interactable = (currentDetailIndex > 0);
        if (btnNextMission != null) btnNextMission.interactable = (currentDetailIndex < todaysMissions.Count - 1);
    }

    public void BackToList()
    {
        missionDetailPanel.SetActive(false);
        missionListPanel.SetActive(true);
    }

    public void StartMission()
    {
        canvasMenu.SetActive(false);
        if (environment3D != null) environment3D.SetActive(false);
        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (endDayButton != null) endDayButton.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetupDailyMissions(todaysMissions);
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.RollRandomEvent();
        }

        if (MiniGameManager.Instance != null)
        {
            MiniGameManager.Instance.StartMiniGame();
        }
    }
}