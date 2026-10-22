using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Top Bar UI")]
    public Text txtMoney;
    public Slider sliderTrust;
    public Text txtTrustPercent; // Opsional

    [Header("Other UI")]
    public Button endDayButton;
    public GameObject summaryPanel;
    public SummaryManager summaryManager;
    public GameObject notifRedDot;

    void Start()
    {
        if (endDayButton != null) endDayButton.gameObject.SetActive(false);
        endDayButton.onClick.AddListener(OnEndDayClicked);
        if (summaryPanel != null) summaryPanel.SetActive(false);

        // Update HUD saat game pertama kali dimulai
        UpdateHUD();
        UpdateReportNotif();
    }

    // Fungsi untuk memperbarui Uang dan Trust di layar
    public void UpdateHUD()
    {
        if (GameManager.Instance != null)
        {
            // 1. Update Uang
            if (txtMoney != null)
                txtMoney.text = $"${GameManager.Instance.currentMoney}";

            // 2. Update Bar Trust
            if (sliderTrust != null)
                sliderTrust.value = GameManager.Instance.currentTrust;

            // 3. Update Teks Persentase (Opsional)
            if (txtTrustPercent != null)
                txtTrustPercent.text = $"{GameManager.Instance.currentTrust}%";
        }
    }

    public void UpdateReportNotif()
    {
        if (notifRedDot != null && GameManager.Instance != null)
        {
            bool shouldShow = (GameManager.Instance.activeMission != null && !GameManager.Instance.isMissionCompleted);
            notifRedDot.SetActive(shouldShow);
        }
    }

    void OnEndDayClicked()
    {
        if (!GameManager.Instance.isMissionCompleted)
        {
            Debug.Log("Misi belum selesai!");
            return;
        }

        endDayButton.gameObject.SetActive(false);

        if (summaryManager != null)
        {
            summaryManager.ShowSummary();
        }
    }
}