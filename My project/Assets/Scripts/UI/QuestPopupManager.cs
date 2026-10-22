using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public Text titleText;
    public Text costText;
    public Button kirimButton;
    public Text logText;
    public CameraController cameraController;
    public TruckManager truckManager;

    [Header("Selected Items Box (Dua Kotak)")]
    public GameObject boxBeras;
    public Text txtBoxBeras;
    public GameObject boxObat;
    public Text txtBoxObat;

    private MissionData currentMission;
    private int totalCost = 0;

    private int countBeras = 0;
    private int countObat = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        popupPanel.SetActive(false);
        // Pastikan kedua kotak mati di awal
        if (boxBeras != null) boxBeras.SetActive(false);
        if (boxObat != null) boxObat.SetActive(false);
    }

    public void OpenPopup()
    {
        // 1. CEK KONDISI: Jika tidak ada misi aktif ATAU misi sudah selesai
        if (GameManager.Instance.activeMission == null || GameManager.Instance.isMissionCompleted)
        {
            if (logText != null)
                logText.text = "Kurir: 'Tidak ada report yang tersedia saat ini. Silakan End Day.'";

            Debug.Log("Tidak ada report yang tersedia.");
            return; // Hentikan fungsi di sini, jangan buka popup
        }

        // 2. JIKA ADA MISI AKTIF, LANJUTKAN SEPERTI BIASA
        currentMission = GameManager.Instance.activeMission;

        // Reset pilihan
        countBeras = 0;
        countObat = 0;
        totalCost = 0;
        if (boxBeras != null) boxBeras.SetActive(false);
        if (boxObat != null) boxObat.SetActive(false);

        // Zoom Kamera
        QuestMarker targetMarker = null;
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == currentMission && m.gameObject.activeInHierarchy)
            {
                targetMarker = m;
                break;
            }
        }
        if (targetMarker != null && cameraController != null)
            cameraController.ZoomTo(targetMarker.transform.position);

        // UI
        titleText.text = $"REPORT 1\n{currentMission.missionTitle}\nApa yang ingin dikirim?";
        popupPanel.SetActive(true);
        if (logText != null) logText.text = "Pilih barang lalu klik Kirim.";

        UpdateUI();
    }

    public void OnItemAmountChanged(string itemName, int amountChange)
    {
        if (itemName == "Beras") countBeras += amountChange;
        if (itemName == "Obat") countObat += amountChange;

        totalCost = (countBeras * currentMission.hargaBeras) + (countObat * currentMission.hargaObat);

        UpdateUI();
        UpdateSelectedItemBoxes(); // Panggil fungsi update dua kotak
    }

    void UpdateSelectedItemBoxes()
    {
        // Logika Box Beras
        if (boxBeras != null && txtBoxBeras != null)
        {
            if (countBeras > 0)
            {
                boxBeras.SetActive(true);
                txtBoxBeras.text = countBeras.ToString();
            }
            else
            {
                boxBeras.SetActive(false);
            }
        }

        // Logika Box Obat
        if (boxObat != null && txtBoxObat != null)
        {
            if (countObat > 0)
            {
                boxObat.SetActive(true);
                txtBoxObat.text = countObat.ToString();
            }
            else
            {
                boxObat.SetActive(false);
            }
        }
    }

    void UpdateUI()
    {
        if (costText != null) costText.text = $"Total Biaya: ${totalCost}";

        bool isMoneyEnough = GameManager.Instance.currentMoney >= totalCost;
        bool isSomethingSelected = (countBeras > 0 || countObat > 0);

        if (kirimButton != null)
            kirimButton.interactable = isMoneyEnough && isSomethingSelected;
    }

    public void OnKirimClicked()
    {
        if (totalCost > GameManager.Instance.currentMoney) return;

        // 1. Potong Uang & Tutup Popup
        GameManager.Instance.currentMoney -= totalCost;
        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();

        // 2. Validasi (Puzzle)
        bool isCorrect = false;
        if (currentMission.correctItemName == "Beras")
            isCorrect = (countBeras == currentMission.correctAmount && countObat == 0);
        else if (currentMission.correctItemName == "Obat")
            isCorrect = (countObat == currentMission.correctAmount && countBeras == 0);

        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();

        // 3. Eksekusi Hasil
        if (isCorrect)
        {
            // --- JIKA BENAR ---
            GameManager.Instance.CompleteMission(currentMission, totalCost); // Ini otomatis set isMissionCompleted = true & isi data Summary
            if (logText != null) logText.text = "Warga: 'Terima kasih banyak! Bantuanmu sangat tepat.'";

            // Mainkan animasi ceklis
            foreach (var m in allMarkers)
            {
                if (m.missionToComplete == currentMission) m.PlaySuccessAnimation();
            }
        }
        else
        {
            // --- JIKA SALAH ---
            GameManager.Instance.currentTrust -= 10;
            if (logText != null) logText.text = "Warga: 'Ini bukan yang kami butuhkan...' (Trust -10%)";

            // Set data manual untuk Summary karena gagal
            GameManager.Instance.isMissionCompleted = true;
            GameManager.Instance.lastMissionCost = totalCost;
            GameManager.Instance.lastMissionProfit = 0;
            GameManager.Instance.lastMissionTrustGained = -10;
            GameManager.Instance.lastMissionTitle = currentMission.missionTitle;

            // Matikan tanda seru LANGSUNG TANPA CEKLIS
            foreach (var m in allMarkers)
            {
                if (m.missionToComplete == currentMission) m.gameObject.SetActive(false);
            }
        }

        // 4. Hilangkan titik merah di Report Card
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null)
        {
            hud.UpdateReportNotif();
            hud.UpdateHUD(); 
        }

        // 5. Cooldown Truk (Hanya dipanggil sekali)
        if (truckManager != null)
        {
            truckManager.UseTruck();
            StartCoroutine(TruckCooldownRoutine());
        }
    }

    IEnumerator TruckCooldownRoutine()
    {
        yield return new WaitForSeconds(3f);
        if (truckManager != null) truckManager.ResetTrucksForNewDay();
    }

    public void OnBatalkanClicked()
    {
        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();
    }
}