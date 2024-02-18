using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExpressionParser
{
    public static List<string> Parse(string expression)
    {
        var tokens = new List<string>();
        var currentToken = new StringBuilder();

        foreach (var c in expression)
        {
            if (char.IsWhiteSpace(c))
            {
                AddTokenToList(tokens, currentToken);
            }
            else if (char.IsDigit(c) || (c == '.' && currentToken.Length > 0))
            {
                // 현재 토큰이 비어있지 않고 알파벳 문자를 포함하고 있다면, 토큰을 분리합니다.
                if (currentToken.Length > 0 && char.IsLetter(currentToken[currentToken.Length - 1]))
                {
                    AddTokenToList(tokens, currentToken);
                }
                currentToken.Append(c);
            }
            else if (char.IsLetter(c))
            {
                // 현재 토큰이 비어있지 않고 숫자를 포함하고 있다면, 토큰을 분리합니다.
                if (currentToken.Length > 0 && char.IsDigit(currentToken[currentToken.Length - 1]))
                {
                    AddTokenToList(tokens, currentToken);
                }
                currentToken.Append(c);
            }
            else
            {
                AddTokenToList(tokens, currentToken);
                tokens.Add(c.ToString());
            }
        }

        AddTokenToList(tokens, currentToken);
        return tokens;
    }

    private static void AddTokenToList(List<string> tokens, StringBuilder currentToken)
    {
        if (currentToken.Length > 0)
        {
            tokens.Add(currentToken.ToString());
            currentToken.Clear();
        }
    }
    public static bool IsNumber(string input)
    {
        double num;
        return double.TryParse(input, out num);
    }

    public static bool IsAlpha(string input)
    {
        return input.Length > 0 && input.All(char.IsLetter);
    }

    public static bool IsOperator(string input)
    {
        return input == "+" || input == "-" || input == "*" || input == "/";
    }
}