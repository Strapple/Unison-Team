using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    [Header("UI References")]
    public GameObject canvasMiniGame;
    public GameObject canvasGameplay;
    public GameObject environment3D;

    [Header("Camera References")]
    public Camera mainCamera;
    public Transform shopViewPoint;
    public Transform mapViewPoint;
    public CameraController cameraController;

    // TAMBAHKAN INI: Referensi ke pergerakan karakter
    [Header("Player Reference")]
    public PlayerMovement playerMovement;

    [Header("Shop UI (Kasir)")]
    public GameObject miniGameUI;
    public Text txtReceipt;
    public Text txtTotalCost;
    public Text txtPlayerMoney;
    public Button btnBuy;
    public Button btnNext;

    private int receiptBeras = 0;
    private int receiptObat = 0;
    private bool isPaid = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (cameraController == null) cameraController = FindObjectOfType<CameraController>();
    }

    void Start()
    {
        btnBuy.onClick.AddListener(OnBuyClicked);
        btnNext.onClick.AddListener(OnNextClicked);
        if (miniGameUI != null) miniGameUI.SetActive(false);
    }

    public void StartMiniGame()
    {
        receiptBeras = 0;
        receiptObat = 0;
        isPaid = false;

        UpdateShopUI();

        // TAMBAHKAN INI: Pastikan karakter bisa jalan saat masuk toko
        if (playerMovement != null) playerMovement.canMove = true;

        if (cameraController != null) cameraController.SetPause(true);

        if (mainCamera != null && shopViewPoint != null)
        {
            mainCamera.transform.position = shopViewPoint.position;
            mainCamera.transform.rotation = shopViewPoint.rotation;
        }

        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (environment3D != null) environment3D.SetActive(false);

        if (canvasMiniGame != null) canvasMiniGame.SetActive(true);
        if (miniGameUI != null) miniGameUI.SetActive(true);

        if (btnBuy != null) btnBuy.gameObject.SetActive(true);
        if (btnNext != null) btnNext.gameObject.SetActive(false);
    }

    public void AddItemToReceipt(string itemName)
    {
        if (isPaid) return;
        if (itemName.Contains("Beras")) receiptBeras++;
        else if (itemName.Contains("Obat")) receiptObat++;
        UpdateShopUI();
    }

    void UpdateShopUI()
    {
        if (txtPlayerMoney != null) txtPlayerMoney.text = $"Uang: ${GameManager.Instance.currentMoney}";
        string receiptText = "STRUK BELANJA:\n";
        int totalCost = 0;

        if (receiptBeras > 0)
        {
            int harga = GameManager.Instance.activeMission.hargaBeras;
            receiptText += $"- Beras x{receiptBeras} = ${harga * receiptBeras}\n";
            totalCost += (receiptBeras * harga);
        }

        if (receiptObat > 0)
        {
            int harga = GameManager.Instance.activeMission.hargaObat;
            receiptText += $"- Obat x{receiptObat} = ${harga * receiptObat}\n";
            totalCost += (receiptObat * harga);
        }

        if (receiptBeras == 0 && receiptObat == 0) receiptText += "(Kosong)";
        if (txtReceipt != null) txtReceipt.text = receiptText;
        if (txtTotalCost != null) txtTotalCost.text = $"Total: ${totalCost}";
    }

    void OnBuyClicked()
    {
        if (receiptBeras == 0 && receiptObat == 0) return;

        int totalCost = (receiptBeras * GameManager.Instance.activeMission.hargaBeras) +
                        (receiptObat * GameManager.Instance.activeMission.hargaObat);

        if (GameManager.Instance.currentMoney >= totalCost)
        {
            isPaid = true;
            GameManager.Instance.currentMoney -= totalCost;
            GameManager.Instance.lastMissionCost = totalCost;

            GameManager.Instance.AddItem("Beras", receiptBeras);
            GameManager.Instance.AddItem("Obat", receiptObat);

            UpdateShopUI();
            if (btnBuy != null) btnBuy.gameObject.SetActive(false);
            if (btnNext != null) btnNext.gameObject.SetActive(true);

            // TAMBAHKAN INI: Bekukan karakter setelah bayar
            if (playerMovement != null) playerMovement.canMove = false;
        }
    }

    void OnNextClicked()
    {
        if (!isPaid) return;

        if (mainCamera != null && mapViewPoint != null)
        {
            mainCamera.transform.position = mapViewPoint.position;
            mainCamera.transform.rotation = mapViewPoint.rotation;
        }

        if (cameraController != null)
        {
            cameraController.SetPause(false);
            cameraController.ResetZoom();
        }

        if (miniGameUI != null) miniGameUI.SetActive(false);
        if (canvasMiniGame != null) canvasMiniGame.SetActive(false);

        if (canvasGameplay != null) canvasGameplay.SetActive(true);
        if (environment3D != null) environment3D.SetActive(true);

        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();
        foreach (var m in allMarkers)
        {
            if (m.missionToComplete == GameManager.Instance.activeMission)
                m.gameObject.SetActive(true);
        }

        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud != null)
        {
            hud.UpdateHUD();
        }
    }
}