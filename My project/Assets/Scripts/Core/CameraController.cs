using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Default View")]
    public Vector3 defaultPosition = new Vector3(0, 20, -20);
    public float defaultSize = 15f;

    [Header("Zoom Settings")]
    public float zoomSize = 5f;
    public float yOffset = 15f;
    public float moveSpeed = 4f;

    private Camera cam;
    private Vector3 targetPosition;
    private float targetSize;
    private bool isZoomed = false;

    // TAMBAHKAN INI
    private bool isPaused = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        defaultPosition = transform.position;
        defaultSize = cam.orthographicSize;

        targetPosition = defaultPosition;
        targetSize = defaultSize;
    }

    void Update()
    {
        // TAMBAHKAN INI: Jika sedang di-pause, jangan gerakkan kamera
        if (isPaused) return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * moveSpeed);
    }

    // TAMBAHKAN FUNGSI BARU INI
    public void SetPause(bool pauseState)
    {
        isPaused = pauseState;
    }

    public void ZoomTo(Vector3 regionWorldPos)
    {
        if (isPaused) return; // Jangan zoom kalau lagi di toko

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