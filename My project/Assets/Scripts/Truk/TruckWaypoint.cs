using UnityEngine;
using System.Collections;
using UnityEngine.Events; // <-- TAMBAHKAN INI

public class TruckWaypoint : MonoBehaviour
{
    [Header("Rute & Pengaturan")]
    public Transform[] waypoints;
    public float baseMoveSpeed = 5f;
    public float waitAtDestination = 3f;

    [Header("Status Kecepatan (Diubah oleh Event)")]
    [Range(0.1f, 3f)]
    public float speedMultiplier = 1f;

    [Header("Event Pemicu")] // <-- TAMBAHKAN INI
    public UnityEvent onTruckFinished;

    private int currentWaypointIndex = 0;
    private float currentMoveSpeed;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;
    }

    public float GetEstimatedTime()
    {
        if (waypoints.Length == 0) return 0f;

        float totalDistance = 0f;
        for (int i = 1; i < waypoints.Length; i++)
        {
            totalDistance += Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);
        }

        float currentSpeed = baseMoveSpeed * speedMultiplier;
        float travelTime = (totalDistance / currentSpeed) + waitAtDestination;
        return travelTime;
    }

    public void StartDelivery()
    {
        gameObject.SetActive(true);
        transform.position = waypoints[0].position;
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;
        StartCoroutine(MoveTruckRoutine());
    }

    IEnumerator MoveTruckRoutine()
    {
        // 1. JALAN DARI KANTOR KE DESA (Maju)
        for (int i = 0; i < waypoints.Length; i++)
        {
            yield return StartCoroutine(MoveToWaypoint(waypoints[i]));
        }

        // 2. SAMPAI DI DESA (Diam sejenak)
        Debug.Log("Truk sampai di desa. Menurunkan barang...");
        yield return new WaitForSeconds(waitAtDestination);

        // 3. JALAN PULANG KE KANTOR (Mundur)
        Debug.Log("Truk pulang ke kantor...");
        for (int i = waypoints.Length - 1; i >= 0; i--)
        {
            yield return StartCoroutine(MoveToWaypoint(waypoints[i]));
        }

        // 4. SEMBUNYIKAN TRUK & PICUL EVENT
        Debug.Log("Truk sampai di kantor dan menghilang.");

        // Memicu event (Memunculkan tombol End Day)
        onTruckFinished?.Invoke();

        gameObject.SetActive(false);
    }

    IEnumerator MoveToWaypoint(Transform targetWaypoint)
    {
        while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}