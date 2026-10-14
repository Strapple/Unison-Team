using UnityEngine;

public class RegionClickHandler : MonoBehaviour
{
    public RegionData data;
    public RegionStatusPanel uiPanel;

    void OnMouseDown()
    {
        Debug.Log("Klik region: " + gameObject.name);
        if (uiPanel != null && data != null)
            uiPanel.Show(data);
    }
}