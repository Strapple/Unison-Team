using UnityEngine;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject reportPopupPanel;

    [Header("UI Elements")]
    public Image imgReportCharacter; // <-- TAMBAHAN UNTUK GAMBAR
    public Text txtTitle;
    public Text txtType;
    public Text txtLocation;
    public Text txtReward;

    void Start()
    {
        if (reportPopupPanel != null) reportPopupPanel.SetActive(false);
    }

    public void OpenReportPopup()
    {
        MissionData mission = GameManager.Instance.activeMission;

        if (mission != null)
        {
            // 1. Set Gambar
            if (imgReportCharacter != null && mission.missionImage != null)
            {
                imgReportCharacter.sprite = mission.missionImage;
            }

            // 2. Set Teks
            txtTitle.text = mission.missionTitle;
            txtType.text = $"Jenis: {mission.missionType}";
            txtLocation.text = $"Lokasi: {mission.locationDesc}";

            // Hitung estimasi reward
            int estReward = mission.profitUang + (mission.correctAmount * mission.hargaBeras);
            txtReward.text = $"REWARD: +${estReward} & {mission.rewardTrust}% Trust";

            // 3. Tampilkan Popup
            reportPopupPanel.SetActive(true);
        }
    }

    public void CloseReportPopup()
    {
        reportPopupPanel.SetActive(false);
    }
}