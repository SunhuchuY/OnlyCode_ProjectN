using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CurrencyType
{
    Diamond,
    Gold,
    MagicStone,
    Exp
}

public class DailyItemData
{
    public string Title;
    public List<(CurrencyType type, int amount)> Rewards = new();
    public (CurrencyType type, int amount) Price;
}

public class ShopPanelUI : MonoBehaviour
{
    public ShopPanelAccessor accessor;

    private ShopDailyItemAccessor dailyItemPrefab;
    private GameObject dailyItemRewardItemPrefab;

    private void OnEnable()
    {
        dailyItemPrefab = Resources.Load<ShopDailyItemAccessor>("Prefab/UI/ShopDailyItem");
        dailyItemRewardItemPrefab = Resources.Load<GameObject>("Prefab/UI/ShopDailyItemRewardItem");

        accessor.DiamondText.text = GameManager.Instance.userDataManager.userData.Diamond.ToString();
        accessor.CloseButton.onClick.AddListener(() => accessor.gameObject.SetActive(false));

        // todo: 마법 뽑기 비용, 버튼 이벤트를 할당해야 합니다.

        // 모든 daily item을 지웁니다.
        accessor.DailyItemsRoot.gameObject.Children()
            .Where(x => x.GetComponent<ShopDailyItemAccessor>() != null)
            .Destroy();

        // temp: 임시적으로 daily item을 추가해 채워 넣습니다.
        CreateDailyItem(new()
        {
            Title = "골드 팩",
            Rewards = new() { (CurrencyType.Gold, 5500), (CurrencyType.MagicStone, 500000) },
            Price = (CurrencyType.Diamond, 100)
        });

        accessor.gameObject.SetActive(false);
    }

    private void CreateDailyItem(DailyItemData _data)
    {
        var _dailyItemAccessor = Instantiate(dailyItemPrefab, accessor.DailyItemsRoot);

        _dailyItemAccessor.TitleText.text = _data.Title;
        _dailyItemAccessor.ButtonText.text = _data.Price.amount.ToString();

        _dailyItemAccessor.Button.onClick.AddListener(() =>
        {
            Debug.Log("구매를 시도합니다.");
            Debug.Log($"title: {_data.Title}, price: {_data.Price.amount}, type: {_data.Price.type}");
        });

        _dailyItemAccessor.RewardBox.gameObject.Children().Destroy();
        _data.Rewards.ForEach(x =>
        {
            var _rewardItem = Instantiate(dailyItemRewardItemPrefab, _dailyItemAccessor.RewardBox);

            _rewardItem.GetComponentInChildren<Image>().sprite =
                Resources.Load<Sprite>($"Sprite/Icon/Currency/i_{x.type.ToString()}");
            _rewardItem.GetComponentInChildren<TextMeshProUGUI>().text = x.amount.ToString();
        });
    }
}