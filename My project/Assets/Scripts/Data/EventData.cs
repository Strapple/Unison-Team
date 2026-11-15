using UnityEngine;

public enum EventType
{
    Hujan,
    Longsor,
    Relawan,
    Donasi
}

[CreateAssetMenu(fileName = "NewEvent", menuName = "P3D/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;
    [TextArea] public string description;
    public EventType eventType;

    [Header("Efek (Sesuaikan dengan Tipe)")]
    [Tooltip("Untuk Hujan/Longsor/Relawan. 1 = Normal, 0.5 = Lambat, 1.5 = Cepat")]
    public float speedMultiplier = 1f;

    [Tooltip("Untuk Donasi. Masukkan nominal bonus uang")]
    public int bonusMoney = 0;
}