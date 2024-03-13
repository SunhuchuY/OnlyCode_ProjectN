using System.Collections;
using System.Numerics;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class UserDataUI : MonoBehaviour
{
    [SerializeField] private CurrencyPanelAccessor currencyPanelAccessor;

    private Tweener diamondTweener;
    private Tweener goldTweener;
    private Tweener bloodstoneTweener;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.Gold)
            .Pairwise()
            .Subscribe(_pair =>
            {
                GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.Gold,
                    _pair.Current - _pair.Previous);
                UpdateGold(_pair.Current);
            })
            .AddTo(gameObject);
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.MagicStone)
            .Pairwise()
            .Subscribe(_pair =>
            {
                GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.MagicStone,
                    _pair.Current - _pair.Previous);
                UpdateMagicstone(_pair.Current);
            })
            .AddTo(gameObject);
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.Diamond)
            .Pairwise()
            .Subscribe(_pair =>
            {
                GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.Diamond,
                    _pair.Current - _pair.Previous);
                UpdateDiamond(_pair.Current);
            })
            .AddTo(gameObject);
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CurrentExp)
            .Pairwise()
            .Subscribe(_pair =>
            {
                GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.Exp,
                    _pair.Current - _pair.Previous);
            })
            .AddTo(gameObject);

        UpdateGold(GameManager.Instance.userDataManager.userData.Gold);
        UpdateMagicstone(GameManager.Instance.userDataManager.userData.MagicStone);
        UpdateDiamond(GameManager.Instance.userDataManager.userData.Diamond);
    }

    private void UpdateGold(BigInteger _current)
    {   
        if (goldTweener != null && goldTweener.IsActive())
        {
            goldTweener.Kill();
            goldTweener = null;
        }

        goldTweener = DOTween.To(
            () => currencyPanelAccessor.GoldText.text,
            x => currencyPanelAccessor.GoldText.text = x,
            CurrencyHelper.ToCurrencyString(_current), 0.5f);
    }

    private void UpdateDiamond(BigInteger _current)
    {
        if (diamondTweener != null && diamondTweener.IsActive())
        {
            diamondTweener.Kill();
            diamondTweener = null;
        }

        diamondTweener = DOTween.To(
            () => currencyPanelAccessor.DiamondText.text,
            x => currencyPanelAccessor.DiamondText.text = x,
            CurrencyHelper.ToCurrencyString(_current), 0.5f);
    }

    private void UpdateMagicstone(BigInteger _current)
    {
        if (bloodstoneTweener != null && bloodstoneTweener.IsActive())
        {
            bloodstoneTweener.Kill();
            bloodstoneTweener = null;
        }

        bloodstoneTweener = DOTween.To(
            () => currencyPanelAccessor.MagicstoneText.text,
            x => currencyPanelAccessor.MagicstoneText.text = x,
            CurrencyHelper.ToCurrencyString(_current), 0.5f);
    }
}