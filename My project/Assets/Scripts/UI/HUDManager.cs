using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Top Bar UI")]
    public Text txtMoney;
    public Slider sliderTrust;
    public Text txtTrustPercent;
    public Text txtDay; // <-- TAMBAHKAN INI (Tarik Teks "DAY 1" ke sini)

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

        UpdateHUD();
        UpdateReportNotif();
    }

    public void UpdateHUD()
    {
        if (GameManager.Instance != null)
        {
            if (txtMoney != null)
                txtMoney.text = $"${GameManager.Instance.currentMoney}";

            if (sliderTrust != null)
                sliderTrust.value = GameManager.Instance.currentTrust;

            if (txtTrustPercent != null)
                txtTrustPercent.text = $"{GameManager.Instance.currentTrust}%";

            // Update Teks Hari
            if (txtDay != null)
                txtDay.text = $"DAY {GameManager.Instance.currentDay}";
        }
    }

    public void UpdateReportNotif()
    {
        if (notifRedDot != null && GameManager.Instance != null)
        {
            bool shouldShow = (GameManager.Instance.GetCurrentMission() != null);
            notifRedDot.SetActive(shouldShow);
        }
    }

    public void ShowEndDayButton()
    {
        if (endDayButton != null) endDayButton.gameObject.SetActive(true);
    }

    public void CheckEndDayCondition()
    {
        if (GameManager.Instance.IsAllMissionsCompleted())
        {
            ShowEndDayButton();
        }
        else
        {
            Debug.Log("Masih ada misi yang tersisa. Tombol End Day belum muncul.");
        }
    }

    void OnEndDayClicked()
    {
        if (!GameManager.Instance.IsAllMissionsCompleted())
        {
            Debug.Log("Misi belum selesai semua!");
            return;
        }

        endDayButton.gameObject.SetActive(false);

        if (summaryManager != null)
        {
            summaryManager.ShowSummary();
        }
    }
}