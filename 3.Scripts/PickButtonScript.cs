using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickButtonScript : MonoBehaviour
{
    private int _skillpickNum = -1;

    public int skillpickNum
    {
        get { return _skillpickNum; }
        set
        {
            _skillpickNum = value;
            CardSetting();
        }
    }

    [SerializeField] Image roundImage, iconImage;
    [SerializeField] TMP_Text rateText;
    [SerializeField] GameObject emptyCardObject, usingCardObject;

    public void PickButton(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;

        GameManager.skillTreeManager.pickbutton_Obj = gameObject;

        GameManager.skillTreeManager.OnInventory();


    }

    public void PickButton()
    {

        if (GameManager.skillTreeManager.isUse)
        {
            skillpickNum = GameManager.skillTreeManager.tempPickIndex;
            GameManager.skillTreeManager.isUse = false;
            return;
        }


        GameManager.skillTreeManager.OnInventory();


    }


    private void CardSetting()
    {
        if (skillpickNum < 0)
            return;

        rateText.text = $"{GameManager.skillTreeManager.cardDatas[skillpickNum].cardCost}";
        roundImage.sprite = GameManager.skillTreeManager.roundSprite[(int)GameManager.skillTreeManager.cardDatas[skillpickNum].cardRate];
        iconImage.sprite = GameManager.skillTreeManager.cardDatas[skillpickNum].cardSprite;
        GameManager.skillTreeManager.CurCardStates[skillpickNum].isHaveCard = true;

        usingCardObject.SetActive(true);
        emptyCardObject.SetActive(false);

    }
}
