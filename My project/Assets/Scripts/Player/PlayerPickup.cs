using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public bool isCarrying = false;
    private string carriedItemType = "";

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Krat_Beras" && !isCarrying)
        {
            isCarrying = true;
            carriedItemType = "Beras";

            if (anim != null) anim.SetBool("isBawaBarang", true);
            Debug.Log("Mengambil Beras");
        }
        else if (other.gameObject.name == "Krat_Obat" && !isCarrying)
        {
            isCarrying = true;
            carriedItemType = "Obat";

            if (anim != null) anim.SetBool("isBawaBarang", true);
            Debug.Log("Mengambil Obat");
        }

        if (other.gameObject.name == "Truk_Area" && isCarrying)
        {
            isCarrying = false;

            if (anim != null)
            {
                anim.SetBool("isBawaBarang", false);
            }

            MiniGameManager.Instance.AddItemToTruck(carriedItemType);
            carriedItemType = "";
        }
    }
}