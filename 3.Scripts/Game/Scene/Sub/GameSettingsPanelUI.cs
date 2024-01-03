using UnityEngine;

public class GameSettingsPanelUI : MonoBehaviour
{
    public GameSettingsPanelAccessor accessor;

    private void OnEnable()
    {
        accessor.BackgroundQuitButton.onClick.AddListener(() => accessor.gameObject.SetActive(false));
        accessor.BgmVolumeSlider.onValueChanged.AddListener(_value =>
        {
            Debug.Log($"BgmVolumeSlider Value Changed: {_value}");
        });
        accessor.SfxVolumeSlider.onValueChanged.AddListener(_value =>
        {
            Debug.Log($"SfxVolumeSlider Value Changed: {_value}");
        });
        accessor.AccountSettingsButton.onClick.AddListener(() =>
        {
            GameManager.Instance.accountSettingsPanelUI.accessor.gameObject.SetActive(true);
        });

        accessor.gameObject.SetActive(false);
    }
}