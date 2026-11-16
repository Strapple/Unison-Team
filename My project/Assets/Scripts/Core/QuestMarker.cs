using UnityEngine;
using System.Collections;

public class QuestMarker : MonoBehaviour
{
    [Header("Data")]
    public MissionData missionToComplete;

    [Header("Rute Truk Spesifik Desa Ini")]
    public Transform[] missionWaypoints; // <-- TAMBAHKAN INI

    [Header("Visual Marker")]
    public GameObject visualExclamation;
    public GameObject visualCheckmark;

    void Start()
    {
        if (visualCheckmark != null) visualCheckmark.SetActive(false);
    }

    public void PlaySuccessAnimation()
    {
        StartCoroutine(SuccessSequence());
    }

    IEnumerator SuccessSequence()
    {
        if (visualExclamation != null) visualExclamation.SetActive(false);
        if (visualCheckmark != null) visualCheckmark.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}