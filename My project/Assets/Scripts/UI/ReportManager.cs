using UnityEngine;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject reportPopupPanel;

    [Header("UI Text")]
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
            txtTitle.text = mission.missionTitle;
            txtType.text = $"Jenis: {mission.missionType}";
            txtLocation.text = $"Lokasi: {mission.locationDesc}";

            // Hitung reward kira-kira
            int estReward = mission.profitUang + (mission.correctAmount * mission.hargaBeras);
            txtReward.text = $"REWARD: +${estReward} & {mission.rewardTrust}% Trust";

            reportPopupPanel.SetActive(true);
        }
    }

    public void CloseReportPopup()
    {
        reportPopupPanel.SetActive(false);
    }
}