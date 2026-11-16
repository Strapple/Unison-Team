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

    public void ShowSummary()
    {
        summaryPanel.SetActive(true);
        summaryPanel.transform.SetAsLastSibling();

        int cost = GameManager.Instance.lastMissionCost;
        int income = GameManager.Instance.lastMissionProfit;
        int trust = GameManager.Instance.lastMissionTrustGained;
        string title = GameManager.Instance.lastMissionTitle;
        int donasi = GameManager.Instance.bonusDonasi;

        int netProfit = (income + donasi) - cost;

        txtMissionTitle.text = "Misi Selesai: " + title;
        txtCost.text = $"Biaya Belanja: -${cost}";

        if (donasi > 0)
            txtRevenue.text = $"Pendapatan Misi: +${income}\n(Donasi Masyarakat: +${donasi})";
        else
            txtRevenue.text = $"Pendapatan Misi: +${income}";

        if (netProfit >= 0)
            txtProfit.text = $"Profit Bersih: +${netProfit}";
        else
            txtProfit.text = $"Kerugian: -${Mathf.Abs(netProfit)}";

        txtTrust.text = $"Perubahan Trust: {trust}%";
    }

    public void OnLanjutClicked()
    {
        Debug.Log("TOMBOL LANJUT BERHASIL DIKLIK!");

        // 1. Maju ke hari berikutnya
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceToNextDay();
        }

        // 2. RESET WAKTU KE 1 PM
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.ResetTime();
        }

        // 3. Panggil fungsi reset UI
        MissionMenuManager menuManager = FindFirstObjectByType<MissionMenuManager>(FindObjectsInactive.Include);

        if (menuManager != null)
        {
            menuManager.ResetToMenu();
        }
        else
        {
            Debug.LogError("MissionMenuManager tidak ditemukan!");
        }

        // 4. Matikan Summary
        summaryPanel.SetActive(false);

        Debug.Log("Berhasil lanjut ke hari berikutnya.");
    }
}