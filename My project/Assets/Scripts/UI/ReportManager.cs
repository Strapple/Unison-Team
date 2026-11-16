using UnityEngine;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject reportPopupPanel;

    [Header("UI Elements")]
    public Image imgReportCharacter;
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
        // Ambil misi yang sedang aktif saat ini
        MissionData mission = GameManager.Instance.GetCurrentMission();

        // Jika tidak ada misi (semua sudah selesai)
        if (mission == null)
        {
            if (imgReportCharacter != null) imgReportCharacter.gameObject.SetActive(false);
            txtTitle.text = "TIDAK ADA REPORT";
            txtType.text = "";
            txtLocation.text = "";
            txtReward.text = "Semua misi hari ini sudah diselesaikan.";

            reportPopupPanel.SetActive(true);
            return;
        }

        // Jika ada misi, tampilkan datanya
        if (imgReportCharacter != null)
        {
            imgReportCharacter.gameObject.SetActive(true);
            if (mission.missionImage != null) imgReportCharacter.sprite = mission.missionImage;
        }

        txtTitle.text = mission.missionTitle;
        txtType.text = $"Jenis: {mission.missionType}";
        txtLocation.text = $"Lokasi: {mission.locationDesc}";

        int estReward = mission.profitUang + (mission.correctAmount * mission.hargaBeras);
        txtReward.text = $"REWARD: +${estReward} & {mission.rewardTrust}% Trust";

        reportPopupPanel.SetActive(true);
    }

    public void CloseReportPopup()
    {
        reportPopupPanel.SetActive(false);
    }
}