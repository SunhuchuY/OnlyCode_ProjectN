using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    private int _cardIndex = -1;

    [HideInInspector] public int cardIndex
    {
        get { return _cardIndex; }
        set
        {
            _cardIndex = value;
            CardSetting();
        }
    }

    [SerializeField] public Image iconImage, roundImage;
    [SerializeField] TMP_Text rateText;

    public void OnCardSystem()
    {
        GameManager.skillTreeManager.PickCardButton(cardIndex);
        Debug.Log("Buutton Test");
    }

    private void CardSetting()
    {
        if (cardIndex < 0)
            return;

        iconImage.sprite = GameManager.skillTreeManager.cardDatas[cardIndex].cardSprite;
        roundImage.sprite = GameManager.skillTreeManager.roundSprite[(int)GameManager.skillTreeManager.cardDatas[cardIndex].cardRate];
        rateText.text = $"{GameManager.skillTreeManager.cardDatas[cardIndex].cardCost}";

    }
}
