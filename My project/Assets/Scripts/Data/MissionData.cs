using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "P3D/Mission Data")]
public class MissionData : ScriptableObject
{
    [Header("Informasi Misi")]
    public string missionTitle = "Bantuan Darurat";
    [TextArea] public string description = "Desa membutuhkan bantuan segera.";
    public Sprite missionImage;

    [Header("Kebutuhan (Beli & Kirim)")]
    public int reqObat = 3;
    public int reqBeras = 1;

    [Header("Harga Satuan")]
    public int hargaObat = 25;
    public int hargaBeras = 10;

    [Header("Reward")]
    public int rewardTrust = 20; // Dalam persen (20%)
    // Uang akan otomatis dikembalikan (balik modal), 
    // tapi kalau mau ada profit, bisa diisi di sini.
    public int profitUang = 0;
}