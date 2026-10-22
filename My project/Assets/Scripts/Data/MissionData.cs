using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "P3D/Mission Data")]
public class MissionData : ScriptableObject
{
    [Header("Informasi Misi")]
    public string missionTitle = "Bantuan Darurat";
    [TextArea] public string description = "Desa membutuhkan bantuan segera.";
    public Sprite missionImage;

    [Header("Detail Laporan")]
    public string missionType = "Danger";
    public string locationDesc = "Tanda Danger di Peta";

    [Header("Harga Satuan Barang")]
    public int hargaBeras = 10;
    public int hargaObat = 25;

    [Header("Reward")]
    public int rewardTrust = 20;
    public int profitUang = 50;

    [Header("Kunci Jawaban (Puzzle)")]
    public string correctItemName = "Obat";
    public int correctAmount = 3;
}