using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Button endDayButton;
    public GameObject summaryPanel;
    public SummaryManager summaryManager;

    void Start()
    {
        // Pastikan baris ini ada
        if (endDayButton != null) endDayButton.gameObject.SetActive(false);

        endDayButton.onClick.AddListener(OnEndDayClicked);
        if (summaryPanel != null) summaryPanel.SetActive(false);
    }

    void OnEndDayClicked()
    {
        if (!GameManager.Instance.isMissionCompleted)
        {
            Debug.Log("Misi belum selesai!");
            return;
        }

        // Sembunyikan tombol End Day
        endDayButton.gameObject.SetActive(false);

        // Panggil SummaryManager untuk menampilkan data
        if (summaryManager != null)
        {
            summaryManager.ShowSummary();
        }
    }
}