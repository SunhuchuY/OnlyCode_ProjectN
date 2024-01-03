using UnityEngine;
using UnityEngine.UI;

public class LeftTopPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button settingsButton2;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button sleepButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button questButton;

    public Button SettingsButton2 => settingsButton2;
    public Button SpeedButton => speedButton;
    public Button SleepButton => sleepButton;
    public Button SettingsButton => settingsButton;
    public Button ShopButton => shopButton;
    public Button QuestButton => questButton;
}