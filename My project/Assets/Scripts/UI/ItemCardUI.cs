using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCardUI : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int currentAmount = 0;
    public int maxAmount = 99;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Klik Kiri: Tambah
            if (currentAmount < maxAmount)
            {
                currentAmount++;
                // Kirim update ke Manager untuk mengurus UI kotak dan biaya
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

    // Fungsi agar angka ter-reset saat popup ditutup/batal
    public void ResetAmount()
    {
        currentAmount = 0;
    }
}