using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    private int _cardIndex = -1;

    [SerializeField] public Image iconImage, roundImage;
    [SerializeField] private TMP_Text rateText;
    [HideInInspector] public int cardIndex
    {
        get { return _cardIndex; }
        set
        {
            _cardIndex = value;
            CardSetting();
        }
    }


    public void OnCardSystem()
    {
        GameManager.Instance.skillTreeManager.PickCardButton(cardIndex);
    }

    private void CardSetting()
    {
        if (cardIndex < 0)
            return;

        iconImage.sprite = GameManager.Instance.skillTreeManager.CardDataContainer.cards[cardIndex].cardSprite;
        roundImage.sprite = GameManager.Instance.skillTreeManager.roundSprite[(int)GameManager.Instance.skillTreeManager.CardDataContainer.cards[cardIndex].cardRate];
        rateText.text = $"{GameManager.Instance.skillTreeManager.CardDataContainer.cards[cardIndex].cardCost}";

    }
}
