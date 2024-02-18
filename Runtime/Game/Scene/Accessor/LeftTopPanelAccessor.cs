using UnityEngine;
using UnityEngine.UI;

public class LeftTopPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button settingsButton2;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button sleepButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button rankingButton;

    public Button SettingsButton2 => settingsButton2;
    public Button SpeedButton => speedButton;
    public Button SleepButton => sleepButton;
    public Button ShopButton => shopButton;
    public Button QuestButton => questButton;
    public Button RankingButton => rankingButton;
}