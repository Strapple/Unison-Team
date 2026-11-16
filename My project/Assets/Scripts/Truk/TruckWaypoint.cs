using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TruckWaypoint : MonoBehaviour
{
    [Header("Rute & Pengaturan")]
    public Transform[] waypoints;
    public float baseMoveSpeed = 5f;
    public float waitAtDestination = 3f;

    [Header("Status Kecepatan (Diubah oleh Event)")]
    [Range(0.1f, 3f)]
    public float speedMultiplier = 1f;

    [Header("Audio")]
    public AudioSource engineAudio; // <-- TAMBAHAN

    [Header("Event Pemicu")]
    public UnityEvent onTruckFinished;

    private float currentMoveSpeed;
    private Transform[] currentRoute;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;
        if (engineAudio != null) engineAudio.playOnAwake = false;
    }

    public float GetEstimatedTime(Transform[] targetWaypoints)
    {
        Transform[] routeToCheck = targetWaypoints;
        if (routeToCheck == null || routeToCheck.Length == 0) routeToCheck = waypoints;
        if (routeToCheck == null || routeToCheck.Length == 0) return 0f;

        float totalDistance = 0f;
        for (int i = 1; i < routeToCheck.Length; i++)
            totalDistance += Vector3.Distance(routeToCheck[i - 1].position, routeToCheck[i].position);

        float currentSpeed = baseMoveSpeed * speedMultiplier;
        return (totalDistance / currentSpeed) + waitAtDestination;
    }

    public void StartDelivery(Transform[] newWaypoints = null)
    {
        if (newWaypoints != null && newWaypoints.Length > 0) currentRoute = newWaypoints;
        else currentRoute = waypoints;

        if (currentRoute == null || currentRoute.Length == 0)
        {
            Debug.LogError("Rute truk kosong! Tidak ada waypoint.");
            return;
        }

        gameObject.SetActive(true);
        transform.position = currentRoute[0].position;
        currentMoveSpeed = baseMoveSpeed * speedMultiplier;

        // MAIN KAN SFX MESIN TRUK
        if (engineAudio != null) engineAudio.Play();

        StartCoroutine(MoveTruckRoutine());
    }

    IEnumerator MoveTruckRoutine()
    {
        for (int i = 0; i < currentRoute.Length; i++)
            yield return StartCoroutine(MoveToWaypoint(currentRoute[i]));

        Debug.Log("Truk sampai di desa. Menurunkan barang...");
        yield return new WaitForSeconds(waitAtDestination);

        for (int i = currentRoute.Length - 1; i >= 0; i--)
            yield return StartCoroutine(MoveToWaypoint(currentRoute[i]));

        Debug.Log("Truk sampai di kantor dan menghilang.");

        // MATIKAN SFX MESIN TRUK
        if (engineAudio != null) engineAudio.Stop();

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