using UnityEngine;
using UnityEngine.UI;

public class TruckManager : MonoBehaviour
{
    [Header("Truck Settings")]
    public int maxTrucks = 1;
    public int availableTrucks = 1;

    [Header("UI Reference")]
    public Text txtTruckCount; // Teks "1/1" di bawah layar

    void Start()
    {
        UpdateUI();
    }

    // Fungsi untuk memakai truk
    public bool UseTruck()
    {
        if (availableTrucks > 0)
        {
            availableTrucks--;
            UpdateUI();
            return true;
        }
        return false;
    }

    // Fungsi untuk mengembalikan truk (cooldown selesai / ganti hari)
    public void ResetTrucksForNewDay()
    {
        availableTrucks = maxTrucks;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (txtTruckCount != null)
            txtTruckCount.text = $"{availableTrucks}/{maxTrucks}";
    }
}