using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class Quest
{
    public readonly string ConditionStr;
    public readonly string CompensationStr;
    public readonly string ColumnStr;
    public readonly Reward reward;
    public readonly Condition condition;

    [JsonConstructor]
    public Quest(string conditionStr, string compensationStr, string columnStr)
    {
        ConditionStr = conditionStr;
        CompensationStr = compensationStr;
        ColumnStr = columnStr;

        condition = new(conditionStr);
        reward = new(compensationStr);
    }

    public class Reward
    {
        public readonly int value;
        public readonly Type type;

        public enum Type
        {
            Diamond,
            Gold
        }

        public Reward(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.Contains("골드"))
            {
                type = Type.Gold;
            }
            else if (text.Contains("다이아"))
            {
                type = Type.Diamond;
            }
            else
            {
                throw new Exception("not founded quest type");
            }

            Match match = Regex.Match(text, @"\d+");

            if (!match.Success)
            {
                throw new Exception("No Number found in the condition");
            }

            value = int.Parse(match.Value);
        }

        public CurrencyType GetCurrencyType()
        {
            switch (type)
            {
                case Type.Diamond:
                    return CurrencyType.Diamond;

                case Type.Gold:
                    return CurrencyType.Gold;

                default:
                    throw new Exception("not found currency Type in Quest.Reward.GetCurrencyType()");
            }

        }
    }

    public class Condition
    {
        public readonly int value;
        public readonly Type type;

        public enum Type
        {
            Join,
            Kill,
            Gacha,
            Story
        }


        public Condition(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.Contains("출석"))
            {
                type = Type.Join;
                return;
            }
            else if (text.Contains("몬스터"))
            {
                type = Type.Kill;
            }
            else if (text.Contains("가챠"))
            {
                type = Type.Gacha;
            }
            else if (text.Contains("에피소드"))
            {
                type = Type.Story;
            }
            else
            {
                throw new Exception("not founded quest type");
            }

            Match _match = Regex.Match(text, @"\d+");

            if (!_match.Success)
            {
                throw new Exception("No Number found in the condition");
            }

            value = int.Parse(_match.Value);
        }
    }
}
