using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public static class ExpressionEvaluator
{
    public static T Evaluate<T>(string _expression, object _context)
    {
        var _expectedType = typeof(T);
        var _evaluated = Evaluate(_expression, _context);

        if (_expectedType == typeof(int))
            return (T)(object)Convert.ToInt32(_evaluated);
        if (_expectedType == typeof(long))
            return (T)(object)Convert.ToInt64(_evaluated);
        if (_expectedType == typeof(float))
            return (T)(object)Convert.ToSingle(_evaluated);
        if (_expectedType == typeof(double))
            return (T)(object)Convert.ToDouble(_evaluated);

        return (T)_evaluated;
    }

    public static object Evaluate(string _expression, object _context)
    {
        _expression = _expression.Trim();
        MatchCollection _matches = Regex.Matches(_expression, @"\%(.*?)\%");

        if (_matches.Count == 1
            && _matches[0].Groups[0].Value.Equals(_expression))
        {
            // 표현식이 "{{variable_path}}"으로만 이루어진 문자열인 경우 이쪽이 실행됩니다.
            // 이것을 객체 그대로 반환하고 종료합니다.
            var _value = GetObjectByPath(_matches[0].Groups[1].Value, _context);
            Assert.IsNotNull(_value, $"변수를 찾을 수 없습니다. {_matches[0].Groups[1].Value}");

            return _value;
        }

        // 그렇지 않은 경우, 수치를 계산하는 계산식으로 간주합니다.
        //   표현식 내 변수들을 수치로 간주합니다.
        //   *계산식 내 변수가 수치가 아닌 경우 계산 오류가 발생합니다.

        string _replaced = _expression;

        foreach (Match _match in _matches)
        {
            object _variableValue = GetObjectByPath(_match.Groups[1].Value, _context);
            Assert.IsNotNull(_variableValue, $"변수를 찾을 수 없습니다. {_match.Groups[1].Value}");

            _replaced = _replaced.Replace(_match.Groups[0].Value, _variableValue.ToString());
        }

        return new System.Data.DataTable().Compute(_replaced, "");
    }

    private static object GetObjectByPath(string _variablePath, object _context)
    {
        var _path = _variablePath.Split(".");

        object _next = _context;
        int i = 0;

        while (i < _path.Length)
        {
            string _nextMemberName = _path[i];

            if (_next is ICustomIndexer _nextIndexer)
            {
                // 커스텀 인덱서가 구현된 객체라면, 커스텀 인덱서로 멤버를 얻습니다.
                // 예: Stats같은 경우, 동적으로 멤버를 갖기 때문에 커스텀 인덱서를 구현해야 했으며, 이를 사용해 멤버를 가져올 수 있습니다.

                object _next2 = _nextIndexer[_nextMemberName];
                if (_next2 == null)
                {
                    // 경로에 대응하는 멤버가 존재하지 않습니다.
                    // 돌아갑니다.
                    break;
                }

                _next = _next2;
                ++i;
                continue;
            }

            var _memberInfo =
                _next.GetType()
                    .GetMember(_path[i],
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .FirstOrDefault(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property);

            if (_memberInfo == null)
            {
                // 경로에 대응하는 멤버가 존재하지 않습니다.
                // 돌아갑니다.
                break;
            }

            if (_memberInfo is FieldInfo _fieldInfo)
                _next = _fieldInfo.GetValue(_next);
            else if (_memberInfo is PropertyInfo _propertyInfo)
                _next = _propertyInfo.GetValue(_next);

            ++i;
        }

        if (i == _path.Length)
        {
            // 경로에 대응하는 멤버 탐색에 성공했습니다.
            return _next;
        }

        // 주어진 경로에 있는 대상을 찾지 못했습니다.
        Debug.LogWarning($"주어진 경로에 있는 대상을 찾지 못했습니다. (variablePath: {_variablePath})");
        return null;
    }
}