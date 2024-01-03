using DG.Tweening;
using System.Numerics;
using Unity.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector2 = UnityEngine.Vector2;

public class AcquireMessageUI : MonoBehaviour
{
    private static readonly string[] CURRENCY_NAMES = new[] { "다이아", "골드", "마석", "경험치" };
    private static readonly Sprite[] CURRENCY_SPRITES = new Sprite[4];

    [SerializeField] private RectTransform messagesParent;
    [SerializeField] private int maxMessageCount = 5;

    private AcquireMessageAccessor[] messages;
    private Sequence[] messageSequences;
    private int index = 0;

    private void Awake()
    {
        messagesParent.gameObject.Children().Destroy();
        messages = new AcquireMessageAccessor[maxMessageCount];
        messageSequences = new Sequence[maxMessageCount];

        var _messagePrefab = Resources.Load<AcquireMessageAccessor>("Prefab/UI/AcquireMessage");
        for (int i = 0; i < maxMessageCount; i++)
        {
            messages[i] = Object.Instantiate(_messagePrefab, messagesParent.transform);
            messages[i].CanvasGroup.alpha = 0f;
        }

        for (int i = 0; i < 4; i++)
        {
            CurrencyType _currencyType = (CurrencyType)i;
            CURRENCY_SPRITES[i] =
                Resources.Load<Sprite>($"Sprite/Icon/Currency/i_{_currencyType.ToString().ToLower()}");
        }
    }

    public void Appear_AccuireBox(CurrencyType _currencyType, BigInteger _value)
    {
        if (index >= maxMessageCount)
        {
            // 짧은 시간 동안 많은 메세지가 생성되면, 가장 오래된 메세지를 새로운 메세지로 대체합니다.
            index = 0;
        }

        int _messageIndex = index++;

        var _message = messages[_messageIndex];
        var _rectTransform = _message.CanvasGroup.GetComponent<RectTransform>();
        Vector2 _originPosition = _rectTransform.anchoredPosition;

        messageSequences[_messageIndex]?.Kill(true);

        _message.Text.text = $"{_value} {CURRENCY_NAMES[(int)_currencyType]}";
        _message.Thumbnail.sprite = CURRENCY_SPRITES[(int)_currencyType];
        _rectTransform.anchoredPosition = _originPosition + Vector2.right * 400;
        _message.CanvasGroup.alpha = 1f;

        messageSequences[_messageIndex] = DOTween.Sequence();
        messageSequences[_messageIndex].Append(_rectTransform.DOAnchorPosX(_originPosition.x, 2f))
            .Append(_message.CanvasGroup.DOFade(0.1f, 1f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (index > _messageIndex)
                    index = _messageIndex;
            });
    }

    public static string[] CurrencyUnits = new string[]
    {
        "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
        "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an",
        "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf",
        "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx",
        "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp",
        "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx",
    };

    /// double 형 데이터를 클리커 게임의 화폐 단위로 표현
    public static string ToCurrencyString(BigInteger number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }
}