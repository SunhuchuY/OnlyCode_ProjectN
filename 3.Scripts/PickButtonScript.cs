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

        GameManager.Instance.skillTreeManager.pickbutton_Obj = gameObject;
            
        GameManager.Instance.skillTreeManager.OnInventory();


    }

    public void PickButton()
    {

        if (GameManager.Instance.skillTreeManager.isUse)
        {
            skillpickNum = GameManager.Instance.skillTreeManager.tempPickIndex;
            GameManager.Instance.skillTreeManager.isUse = false;
            return;
        }


        GameManager.Instance.skillTreeManager.OnInventory();


    }


    private void CardSetting()
    {
        if (skillpickNum < 0)
            return;

        rateText.text = $"{GameManager.Instance.skillTreeManager.cardDatas[skillpickNum].cardCost}";
        roundImage.sprite = GameManager.Instance.skillTreeManager.roundSprite[(int)GameManager.Instance.skillTreeManager.cardDatas[skillpickNum].cardRate];
        iconImage.sprite = GameManager.Instance.skillTreeManager.cardDatas[skillpickNum].cardSprite;
        GameManager.Instance.skillTreeManager.CurCardStates[skillpickNum].isHaveCard = true;

        usingCardObject.SetActive(true);
        emptyCardObject.SetActive(false);

    }
}
