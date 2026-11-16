using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Otomatis menambahkan komponen AudioSource
public class SFXPlayer : MonoBehaviour
{
    [Header("SFX Settings")]
    public AudioClip sfxClip; // Slot untuk memasukkan file suara
    [Range(0f, 1f)] public float volume = 1f;

    [Header("Options")]
    public bool playOnAwake = false; // Centang jika ingin suara langsung main saat objek muncul

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (playOnAwake) PlaySFX();
    }

    // Fungsi utama untuk memutar suara
    public void PlaySFX()
    {
        if (sfxClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(sfxClip, volume);
        }
        else
        {
            Debug.LogWarning("SFX Clip belum dimasukkan di objek: " + gameObject.name);
        }
    }
}