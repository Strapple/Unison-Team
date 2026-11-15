using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        // Ambil data bonus donasi dari GameManager (jika tidak ada event, nilainya 0)
        int donasi = GameManager.Instance.bonusDonasi;

        // Hitung Profit Bersih: (Pendapatan Misi + Donasi) - Biaya Belanja
        int netProfit = (income + donasi) - cost;

        // Update Teks UI
        txtMissionTitle.text = "Misi Selesai: " + title;
        txtCost.text = $"Biaya Belanja: -${cost}";

        // Tampilkan Pendapatan beserta Donasi jika ada
        if (donasi > 0)
        {
            txtRevenue.text = $"Pendapatan Misi: +${income}\n(Donasi Masyarakat: +${donasi})";
        }
        else
        {
            txtRevenue.text = $"Pendapatan Misi: +${income}";
        }

        // Tampilkan hasil akhir (Profit atau Rugi)
        if (netProfit >= 0)
            txtProfit.text = $"Profit Bersih: +${netProfit}";
        else
            txtProfit.text = $"Kerugian: -${Mathf.Abs(netProfit)}";

        txtTrust.text = $"Perubahan Trust: {trust}%";
    }

    public void OnLanjutClicked()
    {
        summaryPanel.SetActive(false);

        // Merestart game (kembali ke Main Menu)
        Debug.Log("Merestart Game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}