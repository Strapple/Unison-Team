using UnityEngine;
using UnityEngine.InputSystem;

public class RegionClickHandler : MonoBehaviour
{
    [Header("Data")]
    public RegionData data;
    public RegionStatusPanel uiPanel;
    public LayerMask regionLayer;
    public CameraController cameraController;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null || cam == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Jangan respon klik kalau kamera sedang zoom
            if (cameraController != null && cameraController.IsZoomed()) return;

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, regionLayer))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Zoom ke region + tampilkan popup
                    if (cameraController != null)
                        cameraController.ZoomTo(transform.position);

                    if (uiPanel != null && data != null)
                        uiPanel.Show(data);
                }
            }
        }
    }
}