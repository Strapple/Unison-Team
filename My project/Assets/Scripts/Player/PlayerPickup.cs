using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Visual Ikon (Di Atas Kepala)")]
    public SpriteRenderer iconRenderer;
    public Sprite spriteBeras;
    public Sprite spriteObat;

    [Header("Audio SFX")]
    public SFXPlayer sfxAmbil; // <-- TAMBAHAN
    public SFXPlayer sfxKasir; // <-- TAMBAHAN

    private bool isCarrying = false;
    private string carriedItemType = "";
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (iconRenderer != null) iconRenderer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Krat_Beras" && !isCarrying) PickUpItem("Beras", spriteBeras);
        else if (other.gameObject.name == "Krat_Obat" && !isCarrying) PickUpItem("Obat", spriteObat);

        if (other.gameObject.name == "Kasir_Area" && isCarrying) DropItem();
    }

    void PickUpItem(string itemType, Sprite itemSprite)
    {
        isCarrying = true;
        carriedItemType = itemType;

        if (anim != null) anim.SetBool("isBawaBarang", true);

        if (iconRenderer != null && itemSprite != null)
        {
            iconRenderer.sprite = itemSprite;
            iconRenderer.gameObject.SetActive(true);
        }

        // MAIN KAN SFX AMBIL
        if (sfxAmbil != null) sfxAmbil.PlaySFX();

        Debug.Log("Mengambil: " + itemType);
    }

    void DropItem()
    {
        isCarrying = false;

        if (anim != null) anim.SetBool("isBawaBarang", false);
        if (iconRenderer != null) iconRenderer.gameObject.SetActive(false);

        MiniGameManager.Instance.AddItemToReceipt(carriedItemType);
        carriedItemType = "";

        // MAIN KAN SFX KASIR
        if (sfxKasir != null) sfxKasir.PlaySFX();

        Debug.Log("Menaruh di Kasir");
    }
}