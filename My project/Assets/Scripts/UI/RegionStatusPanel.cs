using UnityEngine;
using UnityEngine.UI;

public class RegionStatusPanel : MonoBehaviour
{
    public GameObject panel;
    public Text nameText;
    public Text stateText;
    public Text zhiText;
    public Text berasText;
    public Text susuText;
    public Text suplemenText;
    public Button closeButton;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);
    }

    public void Show(RegionData data)
    {
        panel.SetActive(true);
        nameText.text = data.regionName;
        stateText.text = "Status: " + data.state;
        zhiText.text = "ZHI: " + data.zhi;
        berasText.text = "Beras: " + data.stokBeras;
        susuText.text = "Susu: " + data.stokSusu;
        suplemenText.text = "Suplemen: " + data.stokSuplemen;

        switch (data.state)
        {
            case "Tenang": stateText.color = Color.green; break;
            case "Waspada": stateText.color = Color.yellow; break;
            case "Krisis": stateText.color = new Color(1f, 0.5f, 0f); break;
            case "Kolaps": stateText.color = Color.red; break;
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}