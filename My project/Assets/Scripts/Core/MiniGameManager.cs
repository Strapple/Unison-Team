using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    [Header("UI & Camera References")]
    public GameObject canvasMiniGame;
    public GameObject canvasGameplay;
    public GameObject mainCamera;
    public GameObject miniGameCamera;

    [Header("Mini Game UI")]
    public GameObject miniGameUI;
    public Text txtCounter;
    public Button btnFinish;

    [Header("References")]
    public TruckWaypoint truckWaypoint;

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

    public void StartMiniGame()
    {
        countBeras = 0;
        countObat = 0;
        UpdateUI();

        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (mainCamera != null) mainCamera.SetActive(false);

        if (canvasMiniGame != null) canvasMiniGame.SetActive(true);
        if (miniGameCamera != null) miniGameCamera.SetActive(true);

        if (miniGameUI != null) miniGameUI.SetActive(true);

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
        int totalCost = (countBeras * GameManager.Instance.activeMission.hargaBeras) +
                        (countObat * GameManager.Instance.activeMission.hargaObat);

        bool isCorrect = false;
        MissionData mission = GameManager.Instance.activeMission;

        if (mission.correctItemName == "Beras")
            isCorrect = (countBeras == mission.correctAmount && countObat == 0);
        else if (mission.correctItemName == "Obat")
            isCorrect = (countObat == mission.correctAmount && countBeras == 0);

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

        if (miniGameUI != null) miniGameUI.SetActive(false);
        if (canvasMiniGame != null) canvasMiniGame.SetActive(false);
        if (miniGameCamera != null) miniGameCamera.SetActive(false);

        if (canvasGameplay != null) canvasGameplay.SetActive(true);
        if (mainCamera != null) mainCamera.SetActive(true);

        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == mission)
            {
                if (isCorrect) m.PlaySuccessAnimation();
                else m.gameObject.SetActive(false);
            }
        }

        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null)
        {
            hud.UpdateReportNotif();
            hud.UpdateHUD();
        }

        if (truckWaypoint != null)
        {
            truckWaypoint.StartDelivery();
        }
        else
        {
            Debug.LogError("Truk belum dimasukkan ke Inspector MiniGameManager!");
        }
    }
}