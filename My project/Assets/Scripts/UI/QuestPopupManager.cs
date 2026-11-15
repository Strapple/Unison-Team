using UnityEngine;
using UnityEngine.UI;

public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public Text titleText;
    public Text costText; // Kita ubah fungsinya jadi "Total Dikirim"
    public Button kirimButton;
    public Text logText;
    public CameraController cameraController;

    [Header("Selected Items Box")]
    public GameObject boxBeras;
    public Text txtBoxBeras;
    public GameObject boxObat;
    public Text txtBoxObat;
    public Text txtEstimasi; // Tarik Txt_Estimasi ke sini

    [Header("References")]
    public TruckWaypoint truckWaypoint; // <-- TAMBAHKAN INI (Pindahan dari MiniGameManager)

    private MissionData currentMission;
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
        if (boxBeras != null) boxBeras.SetActive(false);
        if (boxObat != null) boxObat.SetActive(false);
    }

    public void OpenPopup()
    {
        if (GameManager.Instance.activeMission == null || GameManager.Instance.isMissionCompleted)
        {
            if (logText != null) logText.text = "Kurir: 'Tidak ada report yang tersedia saat ini.'";
            return;
        }

        currentMission = GameManager.Instance.activeMission;

        // Reset pilihan
        countBeras = 0;
        countObat = 0;
        if (boxBeras != null) boxBeras.SetActive(false);
        if (boxObat != null) boxObat.SetActive(false);

        // Reset ItemCardUI di scene agar angkanya kembali 0
        ItemCardUI[] allCards = Resources.FindObjectsOfTypeAll<ItemCardUI>();
        foreach (var card in allCards) card.ResetAmount();

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
        titleText.text = $"REPORT 1\n{currentMission.missionTitle}\nPilih stok untuk dikirim:";
        popupPanel.SetActive(true);
        if (logText != null) logText.text = "Pilih barang lalu klik Kirim.";

        // Hitung dan tampilkan estimasi waktu
        if (truckWaypoint != null && txtEstimasi != null)
        {
            float estTime = truckWaypoint.GetEstimatedTime();
            txtEstimasi.text = $"Estimasi Waktu: {Mathf.CeilToInt(estTime)} Detik";
        }

        UpdateUI();
    }

    public void OnItemAmountChanged(string itemName, int amountChange)
    {
        if (itemName == "Beras") countBeras += amountChange;
        if (itemName == "Obat") countObat += amountChange;

        UpdateUI();
        UpdateSelectedItemBoxes();
    }

    void UpdateSelectedItemBoxes()
    {
        if (boxBeras != null && txtBoxBeras != null)
        {
            boxBeras.SetActive(countBeras > 0);
            if (countBeras > 0) txtBoxBeras.text = countBeras.ToString();
        }

        if (boxObat != null && txtBoxObat != null)
        {
            boxObat.SetActive(countObat > 0);
            if (countObat > 0) txtBoxObat.text = countObat.ToString();
        }
    }

    void UpdateUI()
    {
        // Ganti teks "Biaya" menjadi "Total Barang"
        if (costText != null) costText.text = $"Total Dikirim: {countBeras + countObat}";

        // Tombol kirim aktif jika ada barang yang dipilih
        bool isSomethingSelected = (countBeras > 0 || countObat > 0);
        if (kirimButton != null) kirimButton.interactable = isSomethingSelected;
    }

    public void OnKirimClicked()
    {
        // 1. Kurangi Stok Inventory
        if (countBeras > 0) GameManager.Instance.RemoveItem("Beras", countBeras);
        if (countObat > 0) GameManager.Instance.RemoveItem("Obat", countObat);

        // 2. Validasi Jawaban
        bool isCorrect = false;
        if (currentMission.correctItemName == "Beras")
            isCorrect = (countBeras == currentMission.correctAmount && countObat == 0);
        else if (currentMission.correctItemName == "Obat")
            isCorrect = (countObat == currentMission.correctAmount && countBeras == 0);

        // 3. Simpan Data untuk Summary & Beri Reward/Penalti
        GameManager.Instance.isMissionCompleted = true;
        GameManager.Instance.lastMissionTitle = currentMission.missionTitle;

        if (isCorrect)
        {
            // Karena uang sudah dipotong di toko, di sini kita hanya memberi reward profit + trust
            // (Catatan: kode ini mengasumsikan uang belanja di toko tidak dikembalikan. Jika ingin dikembalikan, sesuaikan logika di GameManager).
            GameManager.Instance.currentTrust += currentMission.rewardTrust;
            GameManager.Instance.currentMoney += currentMission.profitUang; // Dapat profit bersih

            GameManager.Instance.lastMissionProfit = currentMission.profitUang;
            GameManager.Instance.lastMissionTrustGained = currentMission.rewardTrust;
            if (logText != null) logText.text = "Warga: 'Terima kasih banyak! Bantuanmu sangat tepat.'";
        }
        else
        {
            GameManager.Instance.currentTrust -= 10;
            GameManager.Instance.lastMissionProfit = 0;
            GameManager.Instance.lastMissionTrustGained = -10;
            if (logText != null) logText.text = "Warga: 'Ini bukan yang kami butuhkan...' (Trust -10%)";
        }

        // 4. Tutup Popup & Reset Kamera
        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();

        // 5. Animasi Tanda Seru
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == currentMission)
            {
                if (isCorrect) m.PlaySuccessAnimation();
                else m.gameObject.SetActive(false);
            }
        }

        // 6. Update HUD
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null)
        {
            hud.UpdateReportNotif();
            hud.UpdateHUD();
        }

        // 7. Jalankan Truk (Pindahan dari MiniGameManager)
        if (truckWaypoint != null)
        {
            truckWaypoint.StartDelivery();
        }
        else
        {
            Debug.LogError("TruckWaypoint belum dimasukkan ke Inspector QuestPopupManager!");
        }
    }

    public void OnBatalkanClicked()
    {
        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();
    }
}