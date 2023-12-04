using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Structs : MonoBehaviour
{
    
}

// 코드최적화 필요
[System.Serializable]
public struct CardData
{
    public int cardIndex, cardCost, cardTargetNum ,cardAttackAmount, cardSpawnNum, cardPercent, cardPercent_two;
    public int cardShield;

    public float cardTime, cardDebuff;
    public string cardName, cardExplain;
    public Sprite cardSprite;
    public cardType cardTypeEnum;
    public cardRateEnum cardRate;
    public string factorByLevel;

    [Min(0)] public int[] cardUpgreadeForCardAmount;
    [Min(0)] public int[] cardUpgreadeForNeedMoney; 

    public int GetcardUpgreadeForNeedCardAmount(int cardLevel)
    {
        if (cardLevel >= cardUpgreadeForCardAmount.Length)
            return cardUpgreadeForCardAmount[cardUpgreadeForCardAmount.Length - 1];

        return cardUpgreadeForCardAmount[cardLevel];
    }

    public int GetcardUpgreadeForNeedMoney(int cardLevel)
    {
        if (cardLevel >= cardUpgreadeForNeedMoney.Length)
            return cardUpgreadeForNeedMoney[cardUpgreadeForNeedMoney.Length - 1];

        return cardUpgreadeForNeedMoney[cardLevel];
    }


    public string GetExplainString(int cardLevel)
    {
        if (cardLevel < 0) return null;

        switch (cardTypeEnum)
        {
            case cardType.spawner:
                return ExtractParams(SpawnNumAmount(cardLevel), AttackAmount(cardLevel));
            case cardType.attack:
                return ExtractParams(AttackAmount(cardLevel));
            case cardType.debuff:
                return ExtractParams(SecondsAmount(cardLevel), DebuffAmount(cardLevel));
            case cardType.attackPercent:
                return ExtractParams(SecondsAmount(cardLevel), PercentAmount(cardLevel));
            case cardType.spawnerPercent:
                return ExtractParams(PercentAmount(cardLevel));
            case cardType.shield:
                return ExtractParams(ShieldAmount(cardLevel));
            case cardType.onlyPercent:
                return ExtractParams(PercentAmount(cardLevel));
            case cardType.targetNum:
                return ExtractParams(TargetAmount(cardLevel));
            case cardType.seconds:
                return ExtractParams(SecondsAmount(cardLevel));
            case cardType.percentOfSecond:
                return ExtractParams(PercentAmount(cardLevel), PercentAmount_Two(cardLevel));
        }

        return null;
    }

    private string ExtractParams(params float[] parm)
    {
        var isPram = false;
        var result = "";

        Queue<float> pramsQueue = new Queue<float>();
        for (int i = 0; i < parm.Length; i++) pramsQueue.Enqueue(parm[i]);


        foreach (var ch in cardExplain)
        {
            if (ch == '{')
            {
                isPram = true;
                continue;
            }
            else if(ch == '{')
            {
                isPram = false;
                continue;
            }


            if (!isPram)
                result += ch;
        }

        return result;
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
            return cardTime + (plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardTime * (multiply * level);
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

    public float PercentAmount_Two(int level)
    {
        float sum;


        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            sum = cardPercent_two + (int)(plus * level);
        }
        else
        {
            var multiply = float.Parse(factorByLevel);
            sum = cardPercent_two * (int)(multiply * level);

        }

        if (sum > 100)
            return 100;

        return sum;
    }

    public int ShieldAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardShield + (int)(plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardShield * (int)(multiply * level);
    }

    public int TargetAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return cardTargetNum + (int)(plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return cardTargetNum * (int)(multiply * level);
    }

}


