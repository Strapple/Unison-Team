using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Daftar Event")]
    public EventData[] allEvents;
    [Range(0f, 1f)]
    public float eventChance = 0.35f; // 0.35 = 35%

    [Header("UI Event")]
    public GameObject eventPopupPanel;
    public Text txtEventTitle;
    public Text txtEventDesc;

    [Header("References")]
    public TruckWaypoint truckWaypoint;

    private EventData currentEvent;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (eventPopupPanel != null) eventPopupPanel.SetActive(false);
        if (truckWaypoint == null) truckWaypoint = FindObjectOfType<TruckWaypoint>();
    }

    // Fungsi ini dipanggil saat memulai misi/hari baru
    public void RollRandomEvent()
    {
        // Reset efek dari event sebelumnya
        if (truckWaypoint != null) truckWaypoint.speedMultiplier = 1f;
        GameManager.Instance.bonusDonasi = 0;

        // Roll peluang 35% (0.35)
        float roll = Random.Range(0f, 1f);

        if (roll <= eventChance)
        {
            // Dapat Event!
            currentEvent = allEvents[Random.Range(0, allEvents.Length)];
            ApplyEventEffect(currentEvent);
            ShowEventUI(currentEvent);
        }
        else
        {
            Debug.Log("Tidak ada event hari ini. Cuaca cerah!");
        }
    }

    void ApplyEventEffect(EventData evt)
    {
        if (evt.eventType == EventType.Hujan || evt.eventType == EventType.Longsor || evt.eventType == EventType.Relawan)
        {
            if (truckWaypoint != null)
                truckWaypoint.speedMultiplier = evt.speedMultiplier;
        }
        else if (evt.eventType == EventType.Donasi)
        {
            GameManager.Instance.bonusDonasi = evt.bonusMoney;
        }
    }

    void ShowEventUI(EventData evt)
    {
        if (eventPopupPanel != null)
        {
            txtEventTitle.text = "EVENT HARI INI: " + evt.eventName;
            txtEventDesc.text = evt.description;
            eventPopupPanel.SetActive(true);
        }
    }

    public void CloseEventUI()
    {
        if (eventPopupPanel != null) eventPopupPanel.SetActive(false);
    }
}