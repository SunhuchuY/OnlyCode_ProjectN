using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SkillTreeManager : MonoBehaviour
{
    [HideInInspector] public CurCardState[] CurCardStates;
    [HideInInspector] public int tempPickIndex = -1;
    [SerializeField] private GameObject inventoryObject, cardPrefab;
    [SerializeField] private Color notBuyColor, ableBuyColor;
    [SerializeField] private Image BuyBox;
    [SerializeField] private Transform cardContents_Inventory;
    [SerializeField] private CardDataContainer cardDataContainer;

    private GameObject selectediconObj, selectedpickbutton_Obj;

    public CardDataContainer CardDataContainer => cardDataContainer;

    public Sprite empryIconSprite;
    public Sprite[] roundSprite = new Sprite[3];
    public Transform pickbutton_Parent;

    [Tooltip("This is InventoryInspector")] [SerializeField]
    private InventoryInspector inventoryInspector;

    private bool isClick = false;
    private bool isPossibleBuy;

    private int tempCurCardLevel = 0;

    private void Start()
    {
        // init
        CurCardStates = new CurCardState[cardDataContainer.cards.Length];
        for (int i = 0; i < cardDataContainer.cards.Length; i++)
        {
            CurCardStates[i].cardID = i;
            CurCardStates[i].numOfCard = 0;
            CurCardStates[i].cardLevel = 1;
            CurCardStates[i].isHaveCard = false;

            InventoryCard card = Instantiate(cardPrefab, cardContents_Inventory).GetComponent<InventoryCard>();

            card.cardIndex = i;
        }

        for (int i = 0; i < cardDataContainer.cards.Length; i++)
        {
            cardDataContainer.cards[i].cardUpgreadeForNeedMoney = new int[]
                { 100, 500, 1500, 6000, 18000, 32000, 64000, 128000, 500000 };
            cardDataContainer.cards[i].cardUpgreadeForCardAmount =
                new int[] { 10, 50, 100, 500, 750, 1000, 1250, 1500, 2500 };
        }

        // temp : ī�带 �׽�Ʈ �ؾ��ؼ� 4���� ī�常 Ȱ��ȭ
        for (int k = 0; k < 4; k++)
        {
            GameManager.Instance.skillTreeManager.CurCardStates[k].numOfCard++;
        }

        GameManager.Instance.skillManager.CustomStart();
    }

    public List<int> CanUseCardIDList()
    {
        if (CurCardStates == null)
            return new List<int>() { };

        // ��� �� �� �ִ� ī��� ��ȸ
        List<int> canUseCardIDList = new List<int>();

        foreach (var curCardState in CurCardStates)
        {
            int cardID = curCardState.cardID;

            if (IsHaveCardID(cardID))
                canUseCardIDList.Add(curCardState.cardID);
        }

        return canUseCardIDList;
    }

    public bool IsHaveCardID(int ID)
    {
        if (CurCardStates[ID].isHaveCard)
            return true;
        else
            return false;
    }

    public void OnInventory()
    {
        inventoryObject.SetActive(true);

        for (int i = 0; i < cardDataContainer.cards.Length; i++)
        {
            if (CurCardStates[i].isHaveCard)
                cardContents_Inventory.GetChild(i).gameObject.SetActive(true);
            else
                cardContents_Inventory.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void InventoryExit()
    {
        inventoryObject.SetActive(false);
    }

    public void PickCardButton(int cardIndex)
    {
        inventoryInspector.InventoryInspectorObject.SetActive(true);
        tempPickIndex = cardIndex;

        tempCurCardLevel = CurCardStates[tempPickIndex].cardLevel;
        isPossibleBuy = true;

        inventoryInspector.cardImage.sprite = cardDataContainer.cards[tempPickIndex].cardSprite;
        inventoryInspector.cardIconImage.sprite = cardDataContainer.cards[tempPickIndex].cardSprite;

        var cardLevel = CurCardStates[tempPickIndex].cardLevel;
        var cardCost = cardDataContainer.cards[tempPickIndex].GetCost();

        if (cardDataContainer.cards[tempPickIndex].IsMaxLevel(CurCardStates[tempPickIndex].cardLevel))
        {
            // Max Level
            isPossibleBuy = false;
            inventoryInspector.cardLevelText_Before.text = $"Max Lv.{cardLevel}";
            inventoryInspector.cardLevelText_After.text = $"";
        }
        else
        {
            // Not Max Level
            inventoryInspector.cardLevelText_Before.text = $"Lv.{cardLevel}";
            inventoryInspector.cardLevelText_After.text = $"Lv.{cardLevel + 1}";
        }

        inventoryInspector.cardExplainText_Before.text =
            cardDataContainer.cards[tempPickIndex].GetExplainString(cardLevel);
        inventoryInspector.cardExplainText_After.text =
            cardDataContainer.cards[tempPickIndex].GetExplainString(cardLevel + 1);

        inventoryInspector.cardCostText_Before.text = $"cost : {cardCost}";
        inventoryInspector.cardCostText_After.text = $"cost : {cardCost}";

        if (CurCardStates[tempPickIndex].numOfCard <
            cardDataContainer.cards[tempPickIndex].GetcardUpgreadeForNeedCardAmount(cardLevel))
        {
            inventoryInspector.cardNeedOfCurText.color = Color.red;
            isPossibleBuy = false;
        }
        else
        {
            inventoryInspector.cardNeedOfCurText.color = Color.blue;
        }

        inventoryInspector.cardNeedOfCurText.text = CurCardStates[tempPickIndex].numOfCard.ToString();
        inventoryInspector.cardNeedOfFullText.text =
            $"/{cardDataContainer.cards[tempPickIndex].GetcardUpgreadeForNeedCardAmount(cardLevel)}";

        if (GameManager.Instance.userDataManager.Gold <
            cardDataContainer.cards[tempPickIndex].GetcardUpgreadeForNeedMoney(cardLevel))
        {
            inventoryInspector.curGoldAmountText.color = Color.red;
            isPossibleBuy = false;
        }
        else
        {
            inventoryInspector.curGoldAmountText.color = Color.blue;
        }

        inventoryInspector.curGoldAmountText.text = GameManager.Instance.userDataManager.Gold.ToString();

        if (isPossibleBuy)
        {
            inventoryInspector.lackOfItemText.text = "";
            BuyBox.color = ableBuyColor;
        }
        else
        {
            inventoryInspector.lackOfItemText.text = "재료가 부족합니다.";
            BuyBox.color = notBuyColor;
        }

        inventoryInspector.cardLevelText.text = $"Lv.{tempCurCardLevel}";
        inventoryInspector.cardNameText.text = cardDataContainer.cards[tempPickIndex].cardName;
    }

    public void CardLevelUpButton()
    {
        // 구매 불가능
        if (!isPossibleBuy)
        {
        }
        else // 구매 가능
        {
            GameManager.Instance.userDataManager.Gold +=
                -cardDataContainer.cards[tempPickIndex].GetcardUpgreadeForNeedMoney(tempCurCardLevel);
            CurCardStates[tempPickIndex].numOfCard -=
                cardDataContainer.cards[tempPickIndex].GetcardUpgreadeForNeedCardAmount(tempCurCardLevel);

            CurCardStates[tempPickIndex].cardLevel++;
            PickCardButton(tempPickIndex);
        }
    }
}

[System.Serializable]
class InventoryInspector
{
    public GameObject InventoryInspectorObject;

    public Image cardImage;
    public Image cardIconImage;

    public TMP_Text cardLevelText_Before, cardLevelText_After;
    public TMP_Text cardExplainText_Before, cardExplainText_After;
    public TMP_Text cardCostText_Before, cardCostText_After;
    public TMP_Text lackOfItemText, curGoldAmountText;
    public TMP_Text cardNeedOfCurText, cardNeedOfFullText;
    public TMP_Text cardLevelText, cardNameText;
}

public struct CurCardState
{
    public bool isHaveCard;
    public int cardLevel;
    public int cardID;

    private int _numOfCard;

    public int numOfCard
    {
        get { return _numOfCard; }
        set
        {
            _numOfCard = value;

            if (value >= 1)
                isHaveCard = true;
        }
    }
}