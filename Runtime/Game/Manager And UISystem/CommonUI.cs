using DG.Tweening;
using UnityEngine;

public class CommonUI : MonoBehaviour
{
    [SerializeField] ToastMessageAccessor toastMessageAccessor;

    Sequence informationTween;

    private void Start()
    {
        toastMessageAccessor.MessageBox.gameObject.SetActive(false);
    }

    public void ToastMessage(string _message)
    {
        informationTween?.Kill();
        toastMessageAccessor.MessageText.color =
            new Color(toastMessageAccessor.MessageText.color.r, toastMessageAccessor.MessageText.color.g,
                toastMessageAccessor.MessageText.color.b, 1);
        toastMessageAccessor.MessageText.rectTransform.anchoredPosition = new Vector2(0, 0);

        toastMessageAccessor.MessageText.text = _message;
        informationTween = DOTween.Sequence();
        informationTween
            .OnStart(() => toastMessageAccessor.MessageBox.gameObject.SetActive(true))
            .Append(toastMessageAccessor.MessageText.DOFade(0, 3f))
            .Join(toastMessageAccessor.MessageBox.DOAnchorPosY(10f, 3f))
            .OnComplete(() => toastMessageAccessor.MessageBox.gameObject.SetActive(false));
    }
}