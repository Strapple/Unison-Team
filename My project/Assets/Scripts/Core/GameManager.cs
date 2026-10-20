using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int currentMoney = 300;
    public int currentTrust = 50; // 50%
    public int currentDay = 1;
    public int lastMissionCost = 0;
    public int lastMissionTrustGained = 0;

    [Header("Active Mission")]
    public MissionData activeMission;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMission(MissionData mission)
    {
        activeMission = mission;
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
        lastMissionTrustGained = mission.rewardTrust;

        // Naikkan Trust
        currentTrust += mission.rewardTrust;

        Debug.Log($"Misi Selesai! Uang sekarang: ${currentMoney}, Trust: {currentTrust}%");

        // Di sini nanti kita panggil layar Summary
        // SummaryManager.Instance.ShowSummary(lastMissionCost, lastMissionTrustGained);
    }
}