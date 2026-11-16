using UnityEngine;
using UnityEngine.UI;

public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public Text titleText;
    public Text costText;
    public Text txtEstimasi;
    public Button kirimButton;
    public Text logText;
    public CameraController cameraController;

    [Header("Selected Items Box")]
    public GameObject boxBeras;
    public Text txtBoxBeras;
    public GameObject boxObat;
    public Text txtBoxObat;

    [Header("Audio SFX")]
    public SFXPlayer sfxSenang; // <-- TAMBAHAN
    public SFXPlayer sfxSedih;  // <-- TAMBAHAN

    [Header("References")]
    public TruckWaypoint truckWaypoint;

    private MissionData currentMission;
    private QuestMarker activeMarker;
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
        currentMission = GameManager.Instance.GetCurrentMission();

        if (currentMission == null)
        {
            if (logText != null) logText.text = "Kurir: 'Tidak ada report yang tersedia saat ini.'";
            return;
        }

        countBeras = 0;
        countObat = 0;
        if (boxBeras != null) boxBeras.SetActive(false);
        if (boxObat != null) boxObat.SetActive(false);

        ItemCardUI[] allCards = Resources.FindObjectsOfTypeAll<ItemCardUI>();
        foreach (var card in allCards) card.ResetAmount();

        activeMarker = null;
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == currentMission && m.gameObject.activeInHierarchy)
            {
                activeMarker = m;
                break;
            }
        }

        if (activeMarker != null && cameraController != null)
            cameraController.ZoomTo(activeMarker.transform.position);

        titleText.text = $"REPORT {(GameManager.Instance.currentMissionIndex + 1)}\n{currentMission.missionTitle}\nPilih stok untuk dikirim:";

        if (truckWaypoint != null && txtEstimasi != null && activeMarker != null)
        {
            float estTime = truckWaypoint.GetEstimatedTime(activeMarker.missionWaypoints);
            txtEstimasi.text = $"Estimasi Waktu: {Mathf.CeilToInt(estTime)} Detik";
        }

        popupPanel.SetActive(true);
        if (logText != null) logText.text = "Pilih barang lalu klik Kirim.";

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
        if (costText != null) costText.text = $"Total Dikirim: {countBeras + countObat}";
        bool isSomethingSelected = (countBeras > 0 || countObat > 0);
        if (kirimButton != null) kirimButton.interactable = isSomethingSelected;
    }

    public void OnKirimClicked()
    {
        if (countBeras > 0) GameManager.Instance.RemoveItem("Beras", countBeras);
        if (countObat > 0) GameManager.Instance.RemoveItem("Obat", countObat);

        bool isCorrect = false;
        if (currentMission.correctItemName == "Beras")
            isCorrect = (countBeras == currentMission.correctAmount && countObat == 0);
        else if (currentMission.correctItemName == "Obat")
            isCorrect = (countObat == currentMission.correctAmount && countBeras == 0);

        GameManager.Instance.ProcessMissionOutcome(isCorrect);

        if (isCorrect && logText != null) logText.text = "Warga: 'Terima kasih banyak! Bantuanmu sangat tepat.'";
        else if (!isCorrect && logText != null) logText.text = "Warga: 'Ini bukan yang kami butuhkan...' (Trust -10%)";

        // MAIN KAN SFX BERDASARKAN HASIL
        if (isCorrect)
        {
            if (sfxSenang != null) sfxSenang.PlaySFX();
        }
        else
        {
            if (sfxSedih != null) sfxSedih.PlaySFX();
        }

        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();

        if (activeMarker != null)
        {
            if (isCorrect) activeMarker.PlaySuccessAnimation();
            else activeMarker.gameObject.SetActive(false);
        }

        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null)
        {
            hud.UpdateReportNotif();
            hud.UpdateHUD();
        }

        if (truckWaypoint != null)
        {
            if (activeMarker != null && activeMarker.missionWaypoints.Length > 0)
                truckWaypoint.StartDelivery(activeMarker.missionWaypoints);
            else
                truckWaypoint.StartDelivery(null);
        }
    }

    public void OnBatalkanClicked()
    {
        popupPanel.SetActive(false);
        if (cameraController != null) cameraController.ResetZoom();
    }
}