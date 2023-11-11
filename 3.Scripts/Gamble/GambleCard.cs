using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GambleCard : MonoBehaviour
{
    public Image cardImage;

    public int cardIndex;

    const float ShowCardRotate_Duration = 0.25f;

    bool isOpen;

    private void OnEnable()
    {
        isOpen = false;
    }

    public void ShowCardImage()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(ShowCardImage_Coroutine());
        }

    }

    IEnumerator ShowCardImage_Coroutine()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.DORotate(new Vector3(0, 90, 0), ShowCardRotate_Duration)  // 수정된 부분
            .SetEase(Ease.Linear); // 회전 애니메이션의 이징 설정 (선택사항)

        yield return new WaitForSeconds(ShowCardRotate_Duration);

        cardImage.sprite = GameManager.skillTreeManager.cardDatas[cardIndex].cardSprite;

        transform.DORotate(new Vector3(0, 0, 0), ShowCardRotate_Duration)  // 수정된 부분
            .SetEase(Ease.Linear); // 회전 애니메이션의 이징 설정 (선택사항)
    }
}

[System.Serializable]
public struct CardData
{
    public int cardIndex, cardCost, cardAttackAmount, cardSpawnNum, cardPercent;
    public float cardSeconds, cardDebuff;
    public string cardName, cardExplain;
    public Sprite cardSprite;
    public cardType cardTypeEnum;
    public cardRateEnum cardRate;
    public string factorByLevel;


    [Min(0)] public int[] cardUpgreadeForCardAmount; // 필수 -> size = 10
    [Min(0)] public int[] cardUpgreadeForNeedMoney; // 필수 -> size = 10

    public int GetcardUpgreadeForNeedCardAmount(int cardLevel)
    {
        if (cardLevel >= cardUpgreadeForCardAmount.Length)
            return cardUpgreadeForCardAmount[cardUpgreadeForCardAmount.Length - 1];

        return cardUpgreadeForCardAmount[cardLevel];
    }

    public int GetcardUpgreadeForNeedMoney(int cardLevel)
    {
        if (cardLevel >= cardUpgreadeForNeedMoney.Length)
            return cardUpgreadeForNeedMoney[cardUpgreadeForNeedMoney.Length-1];

        return cardUpgreadeForNeedMoney[cardLevel];
    }


    public string GetExplainString(int cardLevel)
    {
        if (cardLevel < 0) return null;

        switch (cardTypeEnum)
        {
            case cardType.spawner:
                return string.Format(cardExplain, SpawnNumAmount(cardLevel), AttackAmount(cardLevel));
            case cardType.attack:
                return string.Format(cardExplain, AttackAmount(cardLevel));
            case cardType.debuff:
                return string.Format(cardExplain, SecondsAmount(cardLevel), DebuffAmount(cardLevel));
            case cardType.attackPercent:
                return string.Format(cardExplain, SecondsAmount(cardLevel), PercentAmount(cardLevel));
            case cardType.spawnerPercent:
                return string.Format(cardExplain, PercentAmount(cardLevel));

        }

        return null;
    }

    public int GetCost()
    {
        return cardCost;
    }

   public bool IsMaxLevel(int cardLevel)
    {
        if (cardLevel >= cardUpgreadeForCardAmount.Length)
            return true;

        return false;

    }

    public int AttackAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardAttackAmount + (int)(plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardAttackAmount * (int)(multiply * level);
    }


    public float DebuffAmount(int level) 
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardDebuff + (plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardDebuff * (multiply * level);
    }

    public float SecondsAmount(int level) 
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardSeconds + (plus * level);    
        }

        var multiply = float.Parse(factorByLevel);
        return cardSeconds * (multiply * level);
    }

    public int SpawnNumAmount(int level) 
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardSpawnNum + (int)(plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardSpawnNum * (int)(multiply * level);
    }

    public float PercentAmount(int level)
    {
        float sum;


        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            sum = cardPercent + (int)(plus * level);
        }
        else
        {
            var multiply = float.Parse(factorByLevel);
            sum = cardPercent * (int)(multiply * level);

        }

        if (sum > 100)
            return 100;
        
        return sum;
    }
}

public enum cardType
{
    spawner,
    attack,
    debuff,
    attackPercent,
    spawnerPercent
}

public enum MultiplyOrPlus
{
    plus,
    multyply
}