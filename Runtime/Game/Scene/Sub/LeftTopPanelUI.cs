using UnityEngine;

public class LeftTopPanelUI : MonoBehaviour
{
    [SerializeField] private LeftTopPanelAccessor accessor;

    private void Start()
    {
        accessor.SettingsButton2.onClick.AddListener(OnSettingsButtonClick);
        accessor.SpeedButton.onClick.AddListener(() => GameManager.Instance.gameSetting.OnClickSpeedButton());
        accessor.ShopButton.onClick.AddListener(OnShopButtonClick);
        accessor.QuestButton.onClick.AddListener(OnQuestButtonClick);
        accessor.RankingButton.onClick.AddListener(OnRankingButtonClick);
    }

    private void OnDestroy()
    {
        accessor.ShopButton.onClick.RemoveListener(OnShopButtonClick);
        accessor.QuestButton.onClick.RemoveListener(OnQuestButtonClick);
    }

    private void OnSettingsButtonClick()
    {
        GameManager.Instance.gameSettingsPanelUI.accessor.gameObject.SetActive(true);
    }

    private void OnShopButtonClick()
    {
        GameManager.Instance.gachaTf.gameObject.SetActive(true);
    }

    private void OnQuestButtonClick()
    {
        GameManager.Instance.questPanelUI.accessor.gameObject.SetActive(true);
    }

    private void OnRankingButtonClick()
    {
        GameManager.Instance.rankingPanelUI.accessor.gameObject.SetActive(true);
    }
}