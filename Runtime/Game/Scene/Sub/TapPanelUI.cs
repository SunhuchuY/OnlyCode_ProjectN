using DG.Tweening;
using UnityEngine;

public class TapPanelUI : MonoBehaviour
{
    [SerializeField] private TapPanelAccessor Accessor;

    private bool expanded = true;
    private Tweener expandTween;

    private void OnEnable()
    {
        Accessor.ToggleExpandButton.onClick.AddListener(ToggleExpand);
    }

    private void OnDisable()
    {
        Accessor.ToggleExpandButton.onClick.RemoveListener(ToggleExpand);
    }

    private void ToggleExpand()
    {
        if (expandTween != null && expandTween.IsActive())
        {
            expandTween.Kill(true);
            expandTween = null;
        }

        float _targetY = expanded ? -950 : 0;
        expandTween = Accessor.WrappingPanel.DOAnchorPosY(_targetY, 0.3f).SetEase(Ease.OutExpo);
        expanded = !expanded;
    }
}