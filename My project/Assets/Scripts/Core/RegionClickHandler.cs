using UnityEngine;
using UnityEngine.InputSystem;

public class RegionClickHandler : MonoBehaviour
{
    public RegionData data;
    public RegionStatusPanel uiPanel;
    public LayerMask regionLayer; // slot untuk pilih layer "Region"
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            // HANYA raycast ke layer "Region"
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, regionLayer))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (uiPanel != null && data != null)
                        uiPanel.Show(data);
                }
            }
        }
    }
}