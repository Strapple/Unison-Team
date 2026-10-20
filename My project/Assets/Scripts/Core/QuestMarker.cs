using UnityEngine;
using UnityEngine.InputSystem;

public class QuestMarker : MonoBehaviour
{
    public CameraController cameraController;
    public LayerMask questMarkerLayer; // Set ke layer "QuestMarker" di Inspector
    public MissionData missionToComplete; // Misi yang dikaitkan dengan marker ini

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null || cam == null) return;

        // Jangan respon klik kalau kamera sedang zoom
        if (cameraController != null && cameraController.IsZoomed()) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Raycast HANYA ke Layer QuestMarker
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, questMarkerLayer))
            {
                // Jika yang kena adalah objek ini
                if (hit.collider.gameObject == gameObject)
                {
                    OnMarkerClicked();
                }
            }
        }
    }

    void OnMarkerClicked()
    {
        Debug.Log("Tanda seru diklik!");

        // 1. Zoom Kamera ke posisi Tanda Seru
        if (cameraController != null)
            cameraController.ZoomTo(transform.position);

        // 2. Nanti di sini kita panggil UI Popup Quest
        // QuestUIManager.Instance.ShowPopup(missionToComplete);
    }
}