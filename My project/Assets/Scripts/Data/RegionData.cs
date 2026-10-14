using UnityEngine;

[CreateAssetMenu(fileName = "NewRegion", menuName = "P3D/Region Data")]
public class RegionData : ScriptableObject
{
    public string regionName = "Desa Baru";
    [Range(0, 100)] public int zhi = 100;
    public string state = "Tenang";
    public int stokBeras = 100;
    public int stokSusu = 50;
    public int stokSuplemen = 20;
}