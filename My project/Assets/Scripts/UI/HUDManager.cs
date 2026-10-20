using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Button endDayButton;
    public GameObject summaryPanel; // Panel Summary yang nanti kita buat

    void Start()
    {
        // Pastikan baris ini ada
        if (endDayButton != null) endDayButton.gameObject.SetActive(false);

        endDayButton.onClick.AddListener(OnEndDayClicked);
        if (summaryPanel != null) summaryPanel.SetActive(false);
    }

    void OnEndDayClicked()
    {
        // Cek apakah misi sudah selesai
        if (!GameManager.Instance.isMissionCompleted)
        {
            Debug.Log("Misi belum selesai! Selesaikan dulu tanda serunya.");
            // Bisa tambahkan popup warning di sini nanti
            return;
        }

        // Jika selesai, munculkan Summary
        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
        }
    }
}