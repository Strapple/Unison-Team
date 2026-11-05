using UnityEngine;
using System.Collections;

public class TruckWaypoint : MonoBehaviour
{
    [Header("Rute & Pengaturan")]
    public Transform[] waypoints; // Tarik Waypoint_1, 2, 3 ke sini
    public float moveSpeed = 5f;
    public float waitAtDestination = 3f; // Waktu diam di desa

    private int currentWaypointIndex = 0;

    // Fungsi yang dipanggil untuk memulai perjalanan
    public void StartDelivery()
    {
        gameObject.SetActive(true); // Munculkan truk
        transform.position = waypoints[0].position; // Taruh di Kantor
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
        // Selama truk belum sampai di titik waypoint
        while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            // Arahkan truk ke waypoint
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Gerakkan truk
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
            yield return null; // Tunggu frame berikutnya
        }
    }
}