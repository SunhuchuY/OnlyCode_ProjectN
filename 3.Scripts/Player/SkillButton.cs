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
    public bool isDrag = false;

    [SerializeField] public Image iconImage, roundImage;
    [SerializeField] TMP_Text rateText;

    public void BeginDrag()
    {
        isDrag = false;
        GameManager.Instance.skillManager.pickBtn_Num = btnNum;
        GameManager.Instance.skillManager.pickSkill_Num = pickSkillNum;
    }

    private void CardSetting()
    {
        if (pickSkillNum < 0)
        {
            rateText.text = $"";
            iconImage.sprite = GameManager.Instance.skillTreeManager.empryIconSprite;
            return;
        }

        rateText.text = $"{GameManager.Instance.skillTreeManager.cardDatas[pickSkillNum].cardCost}";
        roundImage.sprite = GameManager.Instance.skillTreeManager.roundSprite[(int)GameManager.Instance.skillTreeManager.cardDatas[pickSkillNum].cardRate];
        iconImage.sprite = GameManager.Instance.skillTreeManager.cardDatas[pickSkillNum].cardSprite;

    }
}