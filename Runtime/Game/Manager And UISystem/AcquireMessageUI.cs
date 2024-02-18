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
    private Vector2 startPos;
    private Vector2 endPos;

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

        Vector2 originalPos = messages[0].CanvasGroup.GetComponent<RectTransform>().anchoredPosition;
        startPos = originalPos + Vector2.right * 400;
        endPos = originalPos;
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

        messageSequences[_messageIndex]?.Kill(true);

        _message.Text.text = $"{CurrencyHelper.ToCurrencyString(_value)} {CURRENCY_NAMES[(int)_currencyType]}";
        _message.Thumbnail.sprite = CURRENCY_SPRITES[(int)_currencyType];
        _rectTransform.anchoredPosition = startPos;
        _message.CanvasGroup.alpha = 1f;

        messageSequences[_messageIndex] = DOTween.Sequence();
        messageSequences[_messageIndex]
            .Append(_rectTransform.DOAnchorPosX(endPos.x, 2f))
            .Append(_message.CanvasGroup.DOFade(0f, 1f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (index > _messageIndex)
                    index = _messageIndex;
            });
    }
}