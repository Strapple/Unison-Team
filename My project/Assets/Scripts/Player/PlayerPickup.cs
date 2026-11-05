using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Barang yang Dibawa")]
    public GameObject carriedItem;
    public Transform carryPosition;

    [Header("Prefabs Barang")]
    public GameObject prefabBeras;
    public GameObject prefabObat;

    private void OnTriggerEnter(Collider other)
    {
        // 1. AMBIL BARANG DARI KRAT BERAS
        if (other.gameObject.name == "Krat_Beras" && carriedItem == null)
        {
            GameObject newItem = Instantiate(prefabBeras, carryPosition.position, Quaternion.identity);
            newItem.transform.SetParent(carryPosition);
            carriedItem = newItem;
        }
        // 2. AMBIL BARANG DARI KRAT OBAT
        else if (other.gameObject.name == "Krat_Obat" && carriedItem == null)
        {
            GameObject newItem = Instantiate(prefabObat, carryPosition.position, Quaternion.identity);
            newItem.transform.SetParent(carryPosition);
            carriedItem = newItem;
        }

        // 3. TARUH BARANG DI TRUK
        if (other.gameObject.name == "Truk_Area" && carriedItem != null)
        {
            // Cek barang apa yang sedang dibawa berdasarkan nama Prefab-nya
            string itemName = carriedItem.name.Replace("(Clone)", ""); // Hapus teks (Clone) bawaan Unity

            Destroy(carriedItem);
            carriedItem = null;

            // Kirim info ke MiniGameManager
            MiniGameManager.Instance.AddItemToTruck(itemName);
        }
    }
}