using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface ICardData
{
    string GetExplainString(int cardLevel);
}


// 코드최적화 필요
[System.Serializable]
public class CardData
{
    public int cardIndex, cardCost, cardTargetNum, cardSpawnNum, cardPercent, cardPercent_two;
    public int cardShield;
    public int attackAmount;

    public float secondsAmount;
    public float buffAmount;
    public float debuffAmount;
    public float duration;
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


    public string GetExplainString(int level)
    {
        switch (cardTypeEnum)
        {   
            case cardType.spawner:
                return ChangeFromFormatToPram(SpawnNumAmount(level));
            case cardType.attack:
                Debug.Log(AttackAmount(level));
                return ChangeFromFormatToPram(AttackAmount(level));
            case cardType.debuff:
                return ChangeFromFormatToPram(DebuffAmount(level));
            case cardType.attackPercent:
                return ChangeFromFormatToPram(PercentAmount(level), AttackAmount(level));
            case cardType.spawnerPercent:
                return ChangeFromFormatToPram(PercentAmount(level), SpawnNumAmount(level)) ;
            case cardType.shield:
                return ChangeFromFormatToPram(ShieldAmount(level));
            case cardType.onlyPercent:
                return ChangeFromFormatToPram(PercentAmount(level));
            case cardType.targetNum:
                return ChangeFromFormatToPram(PercentAmount(level));
            case cardType.seconds:
                return ChangeFromFormatToPram(SecondsAmount(level));
            case cardType.percentOfSecond:
                return ChangeFromFormatToPram(PercentAmount(level), PercentAmount_Two(level));
            case cardType.buff:
                return ChangeFromFormatToPram(BuffAmount(level));
            case cardType.debiffSecond:
                return ChangeFromFormatToPram(SecondsAmount(level), DebuffAmount(level));
            default:
                return null;
        }
    }

    public string ChangeFromFormatToPram(params float[] parm)
    {
        var isPram = false;
        var pram = "";
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
            else if(ch == '}')
            {
                isPram = false;
                result += Convert.ToString(pramsQueue.Dequeue());
                pram = "";
                continue;
            }


            if (!isPram)
                result += ch;
        }


        Debug.Log(result);
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

    public float BuffAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return buffAmount + (plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return buffAmount * (multiply * level);
    }

    public float SecondsAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return secondsAmount + (plus * level);
        }

        var multiply = float.Parse(factorByLevel);
        return secondsAmount * (multiply * level);
    }

    public float AttackAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return (attackAmount + (int)(plus * level));
        }
        else
        {
            var multiply = float.Parse(factorByLevel);
            return (attackAmount * (int)(multiply * level));
        }
    }

    public float DebuffAmount(int level)
    {
        if (factorByLevel.EndsWith("+"))
        {
            var plus = float.Parse(factorByLevel.Substring(0, factorByLevel.Length - 1));
            return (debuffAmount + (int)(plus * level));
        }
        else
        {
            var multiply = float.Parse(factorByLevel);
            return (debuffAmount * (int)(multiply * level));
        }
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


