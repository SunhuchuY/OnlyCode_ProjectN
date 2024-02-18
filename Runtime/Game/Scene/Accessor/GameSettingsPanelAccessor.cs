using UnityEngine;
using UnityEngine.UI;

public class GameSettingsPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button backgroundQuitButton;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button accountSettingsButton;

    public Button BackgroundQuitButton => backgroundQuitButton;
    public Slider BgmVolumeSlider => bgmVolumeSlider;
    public Slider SfxVolumeSlider => sfxVolumeSlider;
    public Button AccountSettingsButton => accountSettingsButton;
}