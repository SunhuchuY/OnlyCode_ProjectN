using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

public class CalculateCommandReader
{
    public const string ATK = "atk";
    public const string HP = "hp";

    public static BigInteger GetValue(string text, Monster owner)
    {
        double value = 0;
        List<string> tokens = ExpressionParser.Parse(text);

        if (ExpressionParser.IsOperator(tokens[0]))
        {
            Debug.LogError("첫 번째 연산 명령어가 변수나 숫자가 아닙니다.");
            throw new Exception("The Frist Command is not Number.");
        }

        if (tokens.Count <= 1)
        {
            return (BigInteger)value;
        }

        // 값을 초기화합니다.
        if (ExpressionParser.IsAlpha(tokens[0]))
        {
            switch (tokens[0])
            {
                case ATK:
                    value = (double)owner.attributes.ATK.Value;
                    break;

                case HP:
                    value = (double)owner.attributes.HP.CurrentValue;
                    break;

                default:
                    Debug.LogError("변수를 찾을 수 없습니다.");
                    break;
            }
        }
        else if (ExpressionParser.IsNumber(tokens[0]))
        {
            value = double.Parse(tokens[0]);
        }

        for (int i = 1; i < tokens.Count; i += 2)
        {
            string operatorToken = tokens[i];
            string operandToken = tokens[i + 1];

            if (ExpressionParser.IsAlpha(operandToken))
            {
                switch (operandToken)
                {
                    case ATK:
                        ApplyOperation(ref value, operatorToken, (double)owner.attributes.ATK.Value);
                        break;
                    case HP:
                        ApplyOperation(ref value, operatorToken, (double)owner.attributes.HP.CurrentValue);
                        break;
                    default:
                        throw new Exception("Unknown variable: " + operandToken);
                }
            }
            else if (ExpressionParser.IsNumber(operandToken))
            {
                ApplyOperation(ref value, operatorToken, double.Parse(operandToken));
            }
            else
            {
                throw new Exception("Invalid token: " + operandToken);
            }
        }

        return (BigInteger)value;
    }

    private static void ApplyOperation(ref double currentValue, string operation, double operand)
    {
        switch (operation)
        {
            case "+":
                currentValue += operand;
                break;
            case "-":
                currentValue -= operand;
                break;
            case "*":
                currentValue *= operand;
                break;
            case "/":
                currentValue /= operand;
                break;
            default:
                throw new Exception("Invalid operation: " + operation);
        }
    }
}