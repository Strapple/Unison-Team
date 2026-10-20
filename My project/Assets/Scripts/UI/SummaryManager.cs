using UnityEngine;
using UnityEngine.UI;

public class SummaryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject summaryPanel;
    public Text txtMissionTitle;
    public Text txtCost;
    public Text txtRevenue;
    public Text txtProfit;
    public Text txtTrust;

    void Start()
    {
        if (summaryPanel != null) summaryPanel.SetActive(false);
    }

    // Fungsi ini akan dipanggil oleh HUDManager saat tombol End Day diklik
    public void ShowSummary()
    {
        summaryPanel.SetActive(true);

        int cost = GameManager.Instance.lastMissionCost;
        int profit = GameManager.Instance.lastMissionProfit; // <-- Ambil dari GameManager
        int trust = GameManager.Instance.lastMissionTrustGained;
        string title = GameManager.Instance.lastMissionTitle;

        // Revenue (Pendapatan) adalah modal yang dibalikin + profit
        int revenue = cost + profit;

        // Update Teks UI
        txtMissionTitle.text = "Misi Selesai: " + title;
        txtCost.text = $"Pengeluaran: -${cost}";
        txtRevenue.text = $"Pendapatan: +${revenue}";
        txtProfit.text = $"Profit Bersih: +${profit}"; // <-- Tampilkan profit bersih
        txtTrust.text = $"Kenaikan Trust: +{trust}%";
    }

    // Fungsi untuk tombol Lanjut
    public void OnLanjutClicked()
    {
        summaryPanel.SetActive(false);

        // Di sini nanti kamu bisa memanggil fungsi untuk lanjut ke Day 2
        Debug.Log("Lanjut ke hari berikutnya (Day 2 belum diimplementasi).");
    }
}