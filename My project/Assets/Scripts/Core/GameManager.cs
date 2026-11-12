using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int currentMoney = 300;
    public int currentTrust = 50; // 50%
    public int currentDay = 1;

    [Header("Inventory (Gudang Barang)")]
    public int inventoryBeras = 0;
    public int inventoryObat = 0;

    [Header("Summary Data")]
    public int lastMissionCost = 0;
    public int lastMissionProfit = 0;
    public int lastMissionTrustGained = 0;
    public string lastMissionTitle = "";

    [Header("Active Mission")]
    public MissionData activeMission;
    public bool isMissionCompleted = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMission(MissionData mission)
    {
        activeMission = mission;
        isMissionCompleted = false;
        Debug.Log("Misi dimulai: " + mission.missionTitle);

        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();

        foreach (QuestMarker marker in allMarkers)
        {
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
        lastMissionProfit = mission.profitUang;
        lastMissionTrustGained = mission.rewardTrust;
        lastMissionTitle = mission.missionTitle;

        isMissionCompleted = true;

        currentTrust += mission.rewardTrust;

        // Catatan: Logika pemotongan uang (costSpent) akan kita rombak nanti
        // saat pindah ke sistem Toko. Untuk sekarang, kita biarkan dulu.
        currentMoney += (costSpent + mission.profitUang);

        Debug.Log($"Misi Selesai! Profit didapat: ${mission.profitUang}. Total Uang: ${currentMoney}, Trust: {currentTrust}%");
    }

    // ==========================================
    // FUNGSI BARU UNTUK INVENTORY
    // ==========================================

    // Fungsi untuk menambah barang (Dipanggil oleh Toko setelah bayar)
    public void AddItem(string itemName, int amount)
    {
        if (itemName.Contains("Beras")) inventoryBeras += amount;
        else if (itemName.Contains("Obat")) inventoryObat += amount;

        Debug.Log($"Barang masuk gudang: {itemName} x{amount}. Total Gudang -> Beras: {inventoryBeras}, Obat: {inventoryObat}");
    }

    // Fungsi untuk mengurangi barang (Dipanggil oleh Quest Popup saat kirim)
    public bool RemoveItem(string itemName, int amount)
    {
        if (itemName.Contains("Beras"))
        {
            if (inventoryBeras >= amount)
            {
                inventoryBeras -= amount;
                return true; // Berhasil diambil
            }
        }
        else if (itemName.Contains("Obat"))
        {
            if (inventoryObat >= amount)
            {
                inventoryObat -= amount;
                return true; // Berhasil diambil
            }
        }

        Debug.LogWarning("Gagal mengambil barang: Stok tidak cukup!");
        return false; // Gagal karena stok kurang
    }
}