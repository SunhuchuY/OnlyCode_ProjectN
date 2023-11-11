using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillTreeManager : MonoBehaviour
{
    const int Count = 4;
    const int pickCount = 12; 
    public int GetpickCount() { return pickCount; }

    [SerializeField] Transform cardContents_Inventory;
    public Transform pickbutton_Parent;

    public CardData[] cardDatas; // 전체


    [HideInInspector] public CurCardState[] CurCardStates;

    [HideInInspector] public List<GameObject> pickbutton_ObjList = new List<GameObject>();

    [HideInInspector] public GameObject pickbutton_Obj;

    [HideInInspector] public int tempPickIndex= -1;

    private bool isClick = false;
    [HideInInspector] public bool isUse = false;

    GameObject selectediconObj , selectedpickbutton_Obj;

    [SerializeField] GameObject inventoryObject, cardPrefab , pickSKillBoxObject;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Color notBuyColor, ableBuyColor;

    [SerializeField] Image BuyBox;

    public Sprite empryIconSprite;

    public Sprite[] roundSprite = new Sprite[3];

    [Tooltip("This is InventoryInspector")] [SerializeField] InventoryInspector inventoryInspector;

    bool isPossibleBuy;

    int tempCurCardLevel = 0;

    private void Start()
    {
        // init
        CurCardStates = new CurCardState[cardDatas.Length];
        for (int i = 0; i < cardDatas.Length; i++)
        {
            CurCardStates[i].numOfCard = 0;
            CurCardStates[i].cardLevel = 1;
            CurCardStates[i].isHaveCard = false;

            InventoryCard card = Instantiate(cardPrefab, cardContents_Inventory).GetComponent<InventoryCard>();

            card.cardIndex = i;
        }

        for (int i = 0; i < cardDatas.Length; i++)
        {
            cardDatas[i].cardUpgreadeForNeedMoney = new int[] { 100, 500, 1500, 6000, 18000, 32000, 64000, 128000, 500000 };
            cardDatas[i].cardUpgreadeForCardAmount= new int[] { 10, 50,100, 500, 750, 1000, 1250, 1500, 2500 };
        }

        for (int i = 0; i < pickbutton_Parent.childCount; i++)
        {
            pickbutton_ObjList.Add(pickbutton_Parent.GetChild(i).gameObject);
        }


        //init
        for (int k = 0; k < 4; k++)
        {
            pickbutton_Parent.GetChild(k).GetComponent<PickButtonScript>().skillpickNum = k;
            GameManager.skillTreeManager.CurCardStates[k].numOfCard++;
        }
    }

    public void OnInventory()
    {
        inventoryObject.SetActive(true);

        for (int i = 0; i < cardDatas.Length; i++)
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

        inventoryInspector.cardImage.sprite = cardDatas[tempPickIndex].cardSprite;

        var cardLevel = CurCardStates[tempPickIndex].cardLevel;
        var cardCost = cardDatas[tempPickIndex].GetCost();

        if (cardDatas[tempPickIndex].IsMaxLevel(CurCardStates[tempPickIndex].cardLevel))
        { // Max Level
            isPossibleBuy = false;
            inventoryInspector.cardLevelText_Before.text = $"Max Lv.{cardLevel}";
            inventoryInspector.cardLevelText_After.text = $"";
        }
        else
        { // Not Max Level
            inventoryInspector.cardLevelText_Before.text = $"Lv.{cardLevel}";
            inventoryInspector.cardLevelText_After.text = $"Lv.{cardLevel + 1}";
        }

        inventoryInspector.cardExplainText_Before.text = cardDatas[tempPickIndex].GetExplainString(cardLevel);
        inventoryInspector.cardExplainText_After.text = cardDatas[tempPickIndex].GetExplainString(cardLevel + 1);

        inventoryInspector.cardCostText_Before.text = $"cost : {cardCost}";
        inventoryInspector.cardCostText_After.text = $"cost : {cardCost}";

        if (CurCardStates[tempPickIndex].numOfCard < cardDatas[tempPickIndex].GetcardUpgreadeForNeedCardAmount(cardLevel))
        {
            inventoryInspector.cardNeedOfCurText.color = Color.red;
            isPossibleBuy = false;
        }
        else
        {
            inventoryInspector.cardNeedOfCurText.color = Color.blue;
        }
        inventoryInspector.cardNeedOfCurText.text = CurCardStates[tempPickIndex].numOfCard.ToString();
        inventoryInspector.cardNeedOfFullText.text = $"/{cardDatas[tempPickIndex].GetcardUpgreadeForNeedCardAmount(cardLevel)}";

        if (GameManager.uIManager.GetGold() < cardDatas[tempPickIndex].GetcardUpgreadeForNeedMoney(cardLevel))
        {
            inventoryInspector.curGoldAmountText.color = Color.red;
            isPossibleBuy = false;
        }
        else
        {
            inventoryInspector.curGoldAmountText.color = Color.blue;
        }

        inventoryInspector.curGoldAmountText.text = GameManager.uIManager.GetGold().ToString();

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
        inventoryInspector.cardNameText.text = cardDatas[tempPickIndex].cardName;

        if (IsUseCard(tempPickIndex))
            pickSKillBoxObject.SetActive(false);
        else
            pickSKillBoxObject.SetActive(true);
    }

    public bool IsUseCard(int cardIndex)
    {
        for (int i = 0; i < pickbutton_Parent.childCount; i++)
        {
            if (pickbutton_Parent.GetChild(i).GetComponent<PickButtonScript>().skillpickNum < 0)
                continue;

            if(pickbutton_Parent.GetChild(i).GetComponent<PickButtonScript>().skillpickNum == cardIndex)
            {
                return true;
            }
        }

        return false;
    }

    public void PickUse_Button(BaseEventData data)
    {

        isUse = true;
        InventoryExit();

        if(pickbutton_Obj != null)
           pickbutton_Obj.GetComponent<PickButtonScript>().PickButton();
    }

    public void SkillIcon_Btn(BaseEventData data) // btn
    {
        PointerEventData pointerData = data as PointerEventData;
        selectediconObj = pointerData.pointerPress;

        if (tempPickIndex != selectediconObj.GetComponent<IconpickButton>().skillNum)
        {

            tempPickIndex = selectediconObj.GetComponent<IconpickButton>().skillNum;
            isClick = true;

            return;
        }
    }

    public void CardLevelUpButton()
    {

        // 구매 불가능
        if(!isPossibleBuy)
        {

        }
        else // 구매 가능
        {
            GameManager.uIManager.SetGold(-cardDatas[tempPickIndex].GetcardUpgreadeForNeedMoney(tempCurCardLevel));
            CurCardStates[tempPickIndex].numOfCard -= cardDatas[tempPickIndex].GetcardUpgreadeForNeedCardAmount(tempCurCardLevel);

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

    private int _numOfCard;

    public int numOfCard
    {
        get { return _numOfCard; }
        set
        {
            _numOfCard = value;
            isHaveCard = true;
        }
    }

}
    