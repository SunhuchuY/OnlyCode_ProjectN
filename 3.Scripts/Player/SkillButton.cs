using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private int _pickSkillNum = -1;

    [HideInInspector]
    public int pickSkillNum
    {
        get { return _pickSkillNum; }
        set
        {
            _pickSkillNum = value;
            CardSetting();
        }
    }

    public int btnNum = 0;

    Vector2 originalPickPosition;

    public bool isDrag = false;

    [SerializeField] public Image iconImage, roundImage;
    [SerializeField] TMP_Text rateText;

    public void BeginDrag()
    {
        isDrag = false;
        GameManager.skillManager.pickBtn_Num = btnNum;
        GameManager.skillManager.pickSkill_Num = pickSkillNum;
    }

    private void CardSetting()
    {
        if (pickSkillNum < 0)
        {
            rateText.text = $"";
            iconImage.sprite = GameManager.skillTreeManager.empryIconSprite;
            return;
        }

        rateText.text = $"{GameManager.skillTreeManager.cardDatas[pickSkillNum].cardCost}";
        roundImage.sprite = GameManager.skillTreeManager.roundSprite[(int)GameManager.skillTreeManager.cardDatas[pickSkillNum].cardRate];
        iconImage.sprite = GameManager.skillTreeManager.cardDatas[pickSkillNum].cardSprite;

    }
}
