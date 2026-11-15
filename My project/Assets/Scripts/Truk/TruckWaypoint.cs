using UnityEngine;
using System.Collections;

public class TruckWaypoint : MonoBehaviour
{
    [Header("Rute & Pengaturan")]
    public Transform[] waypoints;
    public float baseMoveSpeed = 5f; // Kecepatan normal
    public float waitAtDestination = 3f; // Waktu diam di desa

    [Header("Status Kecepatan (Diubah oleh Event)")]
    [Range(0.1f, 3f)]
    public float speedMultiplier = 1f; // 1 = Normal, 0.5 = Lambat, 1.5 = Cepat

    private int currentWaypointIndex = 0;
    private float currentMoveSpeed;

    void Start()
    {
        // Set kecepatan awal
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;
    }

    // Fungsi baru: Menghitung estimasi waktu perjalanan (dalam detik)
    public float GetEstimatedTime()
    {
        if (waypoints.Length == 0) return 0f;

        float totalDistance = 0f;
        // Hitung total jarak dari kantor ke desa tujuan
        for (int i = 1; i < waypoints.Length; i++)
        {
            totalDistance += Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);
        }

        // Kecepatan saat ini (base * multiplier)
        float currentSpeed = baseMoveSpeed * speedMultiplier;

        // Waktu tempuh = Jarak / Kecepatan, ditambah waktu diam di desa
        float travelTime = (totalDistance / currentSpeed) + waitAtDestination;

        return travelTime;
    }

    public void StartDelivery()
    {
        gameObject.SetActive(true);
        transform.position = waypoints[0].position;

        // Pastikan kecepatan diperbarui saat berangkat
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

        // 4. SEMBUNYIKAN TRUK
        Debug.Log("Truk sampai di kantor dan menghilang.");
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

            // Gunakan currentMoveSpeed (yang sudah dikali multiplier)
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}