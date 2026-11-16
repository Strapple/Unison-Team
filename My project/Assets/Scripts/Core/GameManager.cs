using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int currentMoney = 300;
    public int currentTrust = 50;
    public int currentDay = 1;

    [Header("Inventory (Gudang Barang)")]
    public int inventoryBeras = 0;
    public int inventoryObat = 0;

    [Header("Summary Data")]
    public int lastMissionCost = 0;
    public int lastMissionProfit = 0;
    public int lastMissionTrustGained = 0;
    public string lastMissionTitle = "";

    [Header("Active Missions (Multi-Misi)")]
    public List<MissionData> dailyMissions = new List<MissionData>();
    public int currentMissionIndex = 0;

    [Header("Random Event Data")]
    public int bonusDonasi = 0;

    void Awake()
    {
        // Singleton pattern agar tidak hancur saat pindah scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <-- TAMBAHKAN INI
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi baru untuk lanjut ke hari berikutnya
    public void AdvanceToNextDay()
    {
        currentDay++;

        // Reset data harian (tapi Uang dan Inventory TETAP)
        dailyMissions.Clear();
        currentMissionIndex = 0;
        lastMissionCost = 0;
        lastMissionProfit = 0;
        lastMissionTrustGained = 0;
        bonusDonasi = 0;

        Debug.Log("Selamat datang di Hari ke-" + currentDay);
    }

    public MissionData GetCurrentMission()
    {
        if (dailyMissions != null && currentMissionIndex < dailyMissions.Count)
            return dailyMissions[currentMissionIndex];
        return null;
    }

    public bool IsAllMissionsCompleted()
    {
        return currentMissionIndex >= dailyMissions.Count;
    }

    public void SetupDailyMissions(List<MissionData> missions)
    {
        dailyMissions = new List<MissionData>(missions);
        currentMissionIndex = 0;
        ActivateMarkerForMission(GetCurrentMission());
    }

    public void ProcessMissionOutcome(bool isCorrect)
    {
        MissionData current = GetCurrentMission();
        if (current == null) return;

        lastMissionTitle = current.missionTitle;

        if (isCorrect)
        {
            lastMissionProfit += current.profitUang;
            lastMissionTrustGained += current.rewardTrust;
            currentTrust += current.rewardTrust;
            currentMoney += current.profitUang;
        }
        else
        {
            lastMissionTrustGained -= 10;
            currentTrust -= 10;
        }

        currentMissionIndex++;

        MissionData nextMission = GetCurrentMission();
        if (nextMission != null)
        {
            ActivateMarkerForMission(nextMission);
        }
    }

    private void ActivateMarkerForMission(MissionData mission)
    {
        if (mission == null) return;
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (QuestMarker marker in allMarkers)
        {
            if (marker.missionToComplete == mission)
            {
                marker.gameObject.SetActive(true);
            }
        }
    }

    public void AddItem(string itemName, int amount)
    {
        if (itemName.Contains("Beras")) inventoryBeras += amount;
        else if (itemName.Contains("Obat")) inventoryObat += amount;
    }

    public bool RemoveItem(string itemName, int amount)
    {
        if (itemName.Contains("Beras"))
        {
            if (inventoryBeras >= amount) { inventoryBeras -= amount; return true; }
        }
        else if (itemName.Contains("Obat"))
        {
            if (inventoryObat >= amount) { inventoryObat -= amount; return true; }
        }
        return false;
    }
}