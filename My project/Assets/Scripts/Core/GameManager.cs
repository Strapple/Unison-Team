using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int currentMoney = 300;
    public int currentTrust = 50; // 50%
    public int currentDay = 1;

    [Header("Summary Data")]
    public int lastMissionCost = 0;
    public int lastMissionProfit = 0;
    public int lastMissionTrustGained = 0;
    public string lastMissionTitle = "";

    [Header("Active Mission")]
    public MissionData activeMission;
    public bool isMissionCompleted = false; // <-- INI YANG BARU DITAMBAHKAN

    void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMission(MissionData mission)
    {
        activeMission = mission;
        isMissionCompleted = false; // Reset status saat misi baru dimulai
        Debug.Log("Misi dimulai: " + mission.missionTitle);

        // Cari semua QuestMarker di dalam scene (termasuk yang sedang mati)
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();

        foreach (QuestMarker marker in allMarkers)
        {
            // Jika misi yang dipilih sama dengan misi di marker, nyalakan markernya
            if (marker.missionToComplete == mission)
            {
                marker.gameObject.SetActive(true);
                Debug.Log("Tanda seru dimunculkan!");
            }
        }
    }

    public void CompleteMission(MissionData mission, int costSpent)
    {
        lastMissionCost = costSpent;
        lastMissionProfit = mission.profitUang; // <-- Ambil profit dari ScriptableObject
        lastMissionTrustGained = mission.rewardTrust;
        lastMissionTitle = mission.missionTitle;

        isMissionCompleted = true;

        // Naikkan Trust
        currentTrust += mission.rewardTrust;

        // UANG: Kembalikan modal (costSpent) + Berikan Profit (profitUang)
        currentMoney += (costSpent + mission.profitUang);

        Debug.Log($"Misi Selesai! Profit didapat: ${mission.profitUang}. Total Uang sekarang: ${currentMoney}, Trust: {currentTrust}%");
    }
}