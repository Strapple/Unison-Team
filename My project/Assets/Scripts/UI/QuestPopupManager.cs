using UnityEngine;
using UnityEngine.UI;

public class QuestPopupManager : MonoBehaviour
{
    public static QuestPopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public Text titleText;
    public CameraController cameraController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        popupPanel.SetActive(false);
    }

    // Fungsi ini dipanggil oleh Tombol Truk
    public void OpenPopup()
    {
        if (GameManager.Instance.activeMission == null)
        {
            Debug.Log("Belum ada misi aktif!");
            return;
        }

        // 1. CARI TANDA SERU YANG AKTIF
        QuestMarker targetMarker = null;
        QuestMarker[] allMarkers = Resources.FindObjectsOfTypeAll<QuestMarker>();

        foreach (var m in allMarkers)
        {
            // Jika marker ini cocok dengan misi aktif dan sedang menyala di scene
            if (m.missionToComplete == GameManager.Instance.activeMission && m.gameObject.activeInHierarchy)
            {
                targetMarker = m;
                break;
            }
        }

        // 2. ZOOM KAMERA KE TANDA SERU
        if (targetMarker != null && cameraController != null)
        {
            cameraController.ZoomTo(targetMarker.transform.position);
        }

        // 3. TAMPILKAN UI
        titleText.text = $"REPORT 1\n{GameManager.Instance.activeMission.missionTitle}\nApa yang ingin dikirim?";
        popupPanel.SetActive(true);
    }

    // Fungsi saat tombol Batal diklik
    public void OnBatalkanClicked()
    {
        popupPanel.SetActive(false);

        // Zoom out kembali ke pandangan awal
        if (cameraController != null)
            cameraController.ResetZoom();
    }
}