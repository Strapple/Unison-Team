using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Default View")]
    public Vector3 defaultPosition = new Vector3(0, 20, -20);
    public float defaultSize = 15f;

    [Header("Zoom Settings")]
    public float zoomSize = 5f;       // ortho size saat zoom in
    public float yOffset = 15f;       // tinggi kamera saat zoom
    public float moveSpeed = 4f;      // kecepatan smooth

    private Camera cam;
    private Vector3 targetPosition;
    private float targetSize;
    private bool isZoomed = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        // Simpan posisi awal sebagai default
        defaultPosition = transform.position;
        defaultSize = cam.orthographicSize;

        targetPosition = defaultPosition;
        targetSize = defaultSize;
    }

    void Update()
    {
        // Smooth lerp posisi & ukuran
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * moveSpeed);
    }

    public void ZoomTo(Vector3 regionWorldPos)
    {
        // Kamera pindah ke atas region, dengan offset Y
        targetPosition = new Vector3(regionWorldPos.x, yOffset, regionWorldPos.z - yOffset);
        targetSize = zoomSize;
        isZoomed = true;
    }

    public void ResetZoom()
    {
        targetPosition = defaultPosition;
        targetSize = defaultSize;
        isZoomed = false;
    }

    public bool IsZoomed() { return isZoomed; }
}