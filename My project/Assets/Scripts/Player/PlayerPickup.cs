using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Visual Ikon (Di Atas Kepala)")]
    public SpriteRenderer iconRenderer; // Komponen untuk menampilkan gambar 2D
    public Sprite spriteBeras;          // Gambar beras
    public Sprite spriteObat;           // Gambar obat

    private bool isCarrying = false;
    private string carriedItemType = "";
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        // Pastikan ikon tidak terlihat di awal
        if (iconRenderer != null) iconRenderer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. AMBIL BARANG DARI RAK
        if (other.gameObject.name == "Krat_Beras" && !isCarrying)
        {
            PickUpItem("Beras", spriteBeras);
        }
        else if (other.gameObject.name == "Krat_Obat" && !isCarrying)
        {
            PickUpItem("Obat", spriteObat);
        }

        // 2. TARUH DI KASIR
        if (other.gameObject.name == "Kasir_Area" && isCarrying)
        {
            DropItem();
        }
    }

    void PickUpItem(string itemType, Sprite itemSprite)
    {
        isCarrying = true;
        carriedItemType = itemType;

        // Mainkan animasi bawa barang (Universal)
        if (anim != null) anim.SetBool("isBawaBarang", true);

        // Tampilkan Ikon di atas kepala
        if (iconRenderer != null && itemSprite != null)
        {
            iconRenderer.sprite = itemSprite;
            iconRenderer.gameObject.SetActive(true);
        }

        Debug.Log("Mengambil: " + itemType);
    }

    void DropItem()
    {
        isCarrying = false;

        // Matikan animasi bawa barang
        if (anim != null) anim.SetBool("isBawaBarang", false);

        // Sembunyikan ikon
        if (iconRenderer != null) iconRenderer.gameObject.SetActive(false);

        // Kirim data ke Toko
        MiniGameManager.Instance.AddItemToReceipt(carriedItemType);
        carriedItemType = "";

        Debug.Log("Menaruh di Kasir");
    }
}