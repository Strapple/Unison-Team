using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    [Header("UI & Camera References")]
    public GameObject canvasMiniGame; // Canvas Mini Game
    public GameObject canvasGameplay; // HUD Peta (Uang, dll)
    public GameObject mainCamera;     // Kamera Peta
    public GameObject miniGameCamera; // Kamera Mini Game

    [Header("Mini Game UI")]
    public GameObject miniGameUI;
    public Text txtCounter;
    public Button btnFinish;

    private int countBeras = 0;
    private int countObat = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        btnFinish.onClick.AddListener(OnFinishClicked);
        if (miniGameUI != null) miniGameUI.SetActive(false);
    }

    // Fungsi untuk MEMULAI mini-game (Dipanggil dari tombol Kirim di Popup)
    public void StartMiniGame()
    {
        countBeras = 0;
        countObat = 0;
        UpdateUI();

        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (mainCamera != null) mainCamera.SetActive(false);

        if (canvasMiniGame != null) canvasMiniGame.SetActive(true);
        if (miniGameCamera != null) miniGameCamera.SetActive(true);

        // Paksa nyalakan panel
        if (miniGameUI != null) miniGameUI.SetActive(true);

        // TAMBAHKAN INI: Paksa nyalakan teks dan tombol
        if (txtCounter != null) txtCounter.gameObject.SetActive(true);
        if (btnFinish != null) btnFinish.gameObject.SetActive(true);
    }

    public void AddItemToTruck(string itemName)
    {
        if (itemName.Contains("Beras")) countBeras++;
        else if (itemName.Contains("Obat")) countObat++;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (txtCounter != null)
            txtCounter.text = $"Beras: {countBeras} | Obat: {countObat}";
    }

    void OnFinishClicked()
    {
        // 1. Hitung Total Biaya
        int totalCost = (countBeras * GameManager.Instance.activeMission.hargaBeras) +
                        (countObat * GameManager.Instance.activeMission.hargaObat);

        // 2. Validasi
        bool isCorrect = false;
        MissionData mission = GameManager.Instance.activeMission;

        if (mission.correctItemName == "Beras")
            isCorrect = (countBeras == mission.correctAmount && countObat == 0);
        else if (mission.correctItemName == "Obat")
            isCorrect = (countObat == mission.correctAmount && countBeras == 0);

        // 3. Eksekusi Data
        GameManager.Instance.currentMoney -= totalCost;
        GameManager.Instance.isMissionCompleted = true;
        GameManager.Instance.lastMissionCost = totalCost;
        GameManager.Instance.lastMissionTitle = mission.missionTitle;

        if (isCorrect)
        {
            GameManager.Instance.currentMoney += (totalCost + mission.profitUang);
            GameManager.Instance.currentTrust += mission.rewardTrust;
            GameManager.Instance.lastMissionProfit = mission.profitUang;
            GameManager.Instance.lastMissionTrustGained = mission.rewardTrust;
        }
        else
        {
            GameManager.Instance.currentTrust -= 10;
            GameManager.Instance.lastMissionProfit = 0;
            GameManager.Instance.lastMissionTrustGained = -10;
        }

        // 4. Transisi Kembali: Mini Game mati, Peta nyala
        if (miniGameUI != null) miniGameUI.SetActive(false);
        if (canvasMiniGame != null) canvasMiniGame.SetActive(false);
        if (miniGameCamera != null) miniGameCamera.SetActive(false);

        if (canvasGameplay != null) canvasGameplay.SetActive(true);
        if (mainCamera != null) mainCamera.SetActive(true);

        // 5. Sembunyikan Tanda Seru
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == mission)
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

        // 7. Nanti di sini kita panggil animasi Truk berjalan
        // TruckWaypoint.Instance.StartDelivery();
    }
}