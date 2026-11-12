using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk memuat ulang scene

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

    public void ShowSummary()
    {
        summaryPanel.SetActive(true);

        int cost = GameManager.Instance.lastMissionCost;
        int income = GameManager.Instance.lastMissionProfit;
        int trust = GameManager.Instance.lastMissionTrustGained;
        string title = GameManager.Instance.lastMissionTitle;

        // Hitung Profit Bersih (Pendapatan dikurangi biaya belanja)
        int netProfit = income - cost;

        // Update Teks UI
        txtMissionTitle.text = "Misi Selesai: " + title;
        txtCost.text = $"Biaya Belanja: -${cost}";

        // Jika misi sukses (income > 0), tampilkan pendapatan. Jika gagal, 0.
        txtRevenue.text = $"Pendapatan Misi: +${income}";

        // Tampilkan hasil akhir (Bisa minus kalau misi gagal/rugi)
        if (netProfit >= 0)
            txtProfit.text = $"Profit Bersih: +${netProfit}";
        else
            txtProfit.text = $"Kerugian: -${Mathf.Abs(netProfit)}";

        txtTrust.text = $"Perubahan Trust: {trust}%";
    }

    public void OnLanjutClicked()
    {
        summaryPanel.SetActive(false);

        // Untuk prototype, kita muat ulang Scene saat ini agar game kembali ke awal
        // (Kembali ke Main Menu dan mereset uang menjadi $300)
        Debug.Log("Merestart Game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}