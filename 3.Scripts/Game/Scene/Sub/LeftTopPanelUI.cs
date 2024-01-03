using System;
using System.Collections;
using UnityEngine;

public class LeftTopPanelUI : MonoBehaviour
{
    [SerializeField] private LeftTopPanelAccessor accessor;

    private void Start()
    {
        accessor.SettingsButton2.onClick.AddListener(OnSettingsButtonClick);
        accessor.SpeedButton.onClick.AddListener(() => GameManager.Instance.gameSetting.OnClickSpeedButton());
        accessor.SettingsButton.onClick.AddListener(OnSettingsButtonClick);
        accessor.ShopButton.onClick.AddListener(OnShopButtonClick);
        accessor.QuestButton.onClick.AddListener(OnQuestButtonClick);
    }

    private void OnDestroy()
    {
        accessor.SettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        accessor.ShopButton.onClick.RemoveListener(OnShopButtonClick);
        accessor.QuestButton.onClick.RemoveListener(OnQuestButtonClick);
    }

    private void OnSettingsButtonClick()
    {
        GameManager.Instance.gameSettingsPanelUI.accessor.gameObject.SetActive(true);
    }

    private void OnShopButtonClick()
    {
        GameManager.Instance.shopPanelUI.accessor.gameObject.SetActive(true);
    }

    private void OnQuestButtonClick()
    {
        throw new NotImplementedException();
    }
}