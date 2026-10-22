using UnityEngine;
using System.Collections;

public class QuestMarker : MonoBehaviour
{
    [Header("Data")]
    public MissionData missionToComplete;

    [Header("Visual Marker")]
    public GameObject visualExclamation; 
    public GameObject visualCheckmark;   

    void Start()
    {
        
        if (visualCheckmark != null) visualCheckmark.SetActive(false);
    }

    // Fungsi yang dipanggil saat misi berhasil diselesaikan
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