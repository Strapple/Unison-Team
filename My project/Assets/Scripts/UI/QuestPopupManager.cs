using UnityEngine;
using UnityEngine.UI;

public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public Text titleText;
    public Text moneyText;
    public Text obatCountText;
    public Text berasCountText;
    public Text costText;
    public Button kirimButton;

    [Header("Camera Reference")]
    public CameraController cameraController; // <-- TAMBAHKAN INI

    private MissionData currentMission;
    private int inputObat = 0;
    private int inputBeras = 0;
    private int totalCost = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        popupPanel.SetActive(false);
    }

    public void OpenPopup(MissionData mission)
    {
        currentMission = mission;
        inputObat = 0;
        inputBeras = 0;

        titleText.text = mission.missionTitle;
        popupPanel.SetActive(true);

        UpdateUI();
    }

    public void AddObat(int amount)
    {
        inputObat = Mathf.Clamp(inputObat + amount, 0, currentMission.reqObat);
        UpdateUI();
    }

    public void AddBeras(int amount)
    {
        inputBeras = Mathf.Clamp(inputBeras + amount, 0, currentMission.reqBeras);
        UpdateUI();
    }

    void UpdateUI()
    {
        moneyText.text = $"Uang: ${GameManager.Instance.currentMoney}";
        obatCountText.text = $"{inputObat} / {currentMission.reqObat}";
        berasCountText.text = $"{inputBeras} / {currentMission.reqBeras}";

        totalCost = (inputObat * currentMission.hargaObat) + (inputBeras * currentMission.hargaBeras);
        costText.text = $"Cost: ${totalCost}";

        bool isMoneyEnough = GameManager.Instance.currentMoney >= totalCost;
        bool isRequirementMet = (inputObat >= currentMission.reqObat) && (inputBeras >= currentMission.reqBeras);

        kirimButton.interactable = isMoneyEnough && isRequirementMet;
    }

    public void OnKirimClicked()
    {
        // Potong uang
        GameManager.Instance.currentMoney -= totalCost;

        // Tutup Popup
        popupPanel.SetActive(false);

        // Reset Kamera
        if (cameraController != null) cameraController.ResetZoom();

        // Beritahu GameManager bahwa misi selesai
        GameManager.Instance.CompleteMission(currentMission, totalCost);

        // Sembunyikan Tanda Seru
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == currentMission) m.gameObject.SetActive(false);
        }
    }

    public void OnBatalkanClicked()
    {
        popupPanel.SetActive(false);

        // Reset Kamera saat Batal
        if (cameraController != null) cameraController.ResetZoom();
    }
}