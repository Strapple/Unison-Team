using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCardUI : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int currentAmount = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Cek stok dari GameManager
        int availableStock = 0;
        if (itemName == "Beras") availableStock = GameManager.Instance.inventoryBeras;
        else if (itemName == "Obat") availableStock = GameManager.Instance.inventoryObat;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Klik Kiri: Tambah (selama stok masih ada)
            if (currentAmount < availableStock)
            {
                currentAmount++;
                QuestPopupManager.Instance.OnItemAmountChanged(itemName, 1);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Klik Kanan: Kurangi
            if (currentAmount > 0)
            {
                currentAmount--;
                QuestPopupManager.Instance.OnItemAmountChanged(itemName, -1);
            }
        }
    }

    public void ResetAmount()
    {
        currentAmount = 0;
    }
}