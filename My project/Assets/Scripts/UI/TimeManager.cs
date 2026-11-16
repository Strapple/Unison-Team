using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("UI References")]
    public Text txtTimeDisplay; // Tarik Teks Jam (1 PM) ke sini

    [Header("Time Settings")]
    public float realSecondsPerGameHour = 120f; // 120 detik = 2 menit = 1 jam game
    public int currentHour = 1;
    public bool isPM = true;

    private float timer = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateTimeUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= realSecondsPerGameHour)
        {
            timer = 0f;
            AdvanceHour();
        }
    }

    void AdvanceHour()
    {
        currentHour++;

        if (currentHour > 12)
        {
            currentHour = 1;
            isPM = !isPM;
        }

        UpdateTimeUI();
    }

    public void UpdateTimeUI()
    {
        if (txtTimeDisplay != null)
        {
            string amPm = isPM ? "PM" : "AM";
            txtTimeDisplay.text = $"{currentHour}\n{amPm}";
        }
    }

    // Fungsi untuk mereset waktu ke 1 PM (dipanggil saat ganti hari)
    public void ResetTime()
    {
        currentHour = 1;
        isPM = true;
        timer = 0f;
        UpdateTimeUI();
        Debug.Log("Waktu telah di-reset ke jam 1 PM.");
    }
}