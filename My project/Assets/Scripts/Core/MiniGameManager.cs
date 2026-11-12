using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    [Header("UI & Camera References")]
    public GameObject canvasMiniGame;
    public GameObject canvasGameplay; // HUD Peta
    public GameObject mainCamera;
    public GameObject miniGameCamera;

    [Header("Gameplay References")]
    public GameObject environment3D;

    [Header("Shop UI (Kasir)")]
    public GameObject miniGameUI;
    public Text txtReceipt;      // Teks untuk daftar belanjaan
    public Text txtTotalCost;    // Teks untuk total harga
    public Text txtPlayerMoney;  // Teks untuk uang pemain saat ini
    public Button btnBuy;        // Tombol Bayar
    public Button btnNext;       // Tombol Lanjut ke Peta (Awalnya mati)

    private int receiptBeras = 0;
    private int receiptObat = 0;
    private bool isPaid = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        btnBuy.onClick.AddListener(OnBuyClicked);
        btnNext.onClick.AddListener(OnNextClicked);

        if (miniGameUI != null) miniGameUI.SetActive(false);
    }

    // Fungsi untuk masuk ke Toko (Dipanggil saat klik Start Misi)
    public void StartMiniGame()
    {
        receiptBeras = 0;
        receiptObat = 0;
        isPaid = false;

        UpdateShopUI();

        if (canvasGameplay != null) canvasGameplay.SetActive(false);
        if (mainCamera != null) mainCamera.SetActive(false);

        if (canvasMiniGame != null) canvasMiniGame.SetActive(true);
        if (miniGameCamera != null) miniGameCamera.SetActive(true);

        if (miniGameUI != null) miniGameUI.SetActive(true);

        // Tampilkan tombol bayar, sembunyikan tombol next
        if (btnBuy != null) btnBuy.gameObject.SetActive(true);
        if (btnNext != null) btnNext.gameObject.SetActive(false);
    }

    // Dipanggil oleh PlayerPickup saat menaruh barang di Kasir
    public void AddItemToReceipt(string itemName)
    {
        if (isPaid) return; // Kalau sudah bayar, jangan bisa nambah barang

        if (itemName.Contains("Beras")) receiptBeras++;
        else if (itemName.Contains("Obat")) receiptObat++;

        UpdateShopUI();
    }

    void UpdateShopUI()
    {
        if (txtPlayerMoney != null) txtPlayerMoney.text = $"Uang: ${GameManager.Instance.currentMoney}";

        // Susun teks struk belanja
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

    // Tombol Bayar di Kasir
    void OnBuyClicked()
    {
        if (receiptBeras == 0 && receiptObat == 0) return; // Tidak bisa bayar kosong

        // Hitung total harga
        int totalCost = (receiptBeras * GameManager.Instance.activeMission.hargaBeras) +
                        (receiptObat * GameManager.Instance.activeMission.hargaObat);

        // Cek uang cukup
        if (GameManager.Instance.currentMoney >= totalCost)
        {
            isPaid = true;

            // Potong Uang & Masukkan ke Inventory
            GameManager.Instance.currentMoney -= totalCost;
            GameManager.Instance.AddItem("Beras", receiptBeras);
            GameManager.Instance.AddItem("Obat", receiptObat);

            UpdateShopUI();

            // Sembunyikan tombol bayar, munculkan tombol Next
            if (btnBuy != null) btnBuy.gameObject.SetActive(false);
            if (btnNext != null) btnNext.gameObject.SetActive(true);

            Debug.Log("Pembayaran berhasil!");
        }
        else
        {
            Debug.Log("Uang tidak cukup!");
            // Bisa tambahkan efek suara error atau teks merah
        }
    }

    // Tombol Lanjut ke Peta
    void OnNextClicked()
    {
        if (!isPaid) return;

        // Matikan Toko
        if (miniGameUI != null) miniGameUI.SetActive(false);
        if (canvasMiniGame != null) canvasMiniGame.SetActive(false);
        if (miniGameCamera != null) miniGameCamera.SetActive(false);

        // Nyalakan Peta dan HUD
        if (canvasGameplay != null) canvasGameplay.SetActive(true);
        if (mainCamera != null) mainCamera.SetActive(true);

        if (environment3D != null) environment3D.SetActive(true); // <-- Pastikan ini ada

        // Nyalakan tanda seru
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
            hud.ShowEndDayButton(); // <-- TAMBAHKAN INI
        }
    }
}