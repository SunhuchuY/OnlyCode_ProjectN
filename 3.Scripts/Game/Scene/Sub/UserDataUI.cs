using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;

public class UserDataUI : MonoBehaviour
{
    [SerializeField] private CurrencyPanelAccessor currencyPanelAccessor;

    private Tweener expTweener;
    private Tweener goldTweener;
    private Tweener bloodstoneTweener;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        GameManager.Instance.userDataManager.OnCurrentExpChanged += UpdateExp;
        GameManager.Instance.userDataManager.OnMagicstoneChanged += UpdateMagicstone;
        GameManager.Instance.userDataManager.OnGoldChanged += UpdateGold;

        currencyPanelAccessor.GoldText.text = GameManager.Instance.userDataManager.Gold.ToString();
        currencyPanelAccessor.MagicstoneText.text = GameManager.Instance.userDataManager.Magicstone.ToString();
        currencyPanelAccessor.DiamondText.text = GameManager.Instance.userDataManager.Diamond.ToString();
    }

    private void UpdateExp(BigInteger _amount)
    {
        if (expTweener != null && expTweener.IsActive())
        {
            expTweener.Kill();
            expTweener = null;
        }

        GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.Exp, _amount);
        expTweener = DOTween.To(
            () => currencyPanelAccessor.MagicstoneText.text,
            x => currencyPanelAccessor.MagicstoneText.text = x,
            GameManager.Instance.userDataManager.Gold.ToString(), 0.5f);
    }

    private void UpdateMagicstone(BigInteger _amount)
    {
        if (bloodstoneTweener != null && bloodstoneTweener.IsActive())
        {
            bloodstoneTweener.Kill();
            bloodstoneTweener = null;
        }

        GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.MagicStone, _amount);
        bloodstoneTweener = DOTween.To(
            () => currencyPanelAccessor.MagicstoneText.text,
            x => currencyPanelAccessor.MagicstoneText.text = x,
            GameManager.Instance.userDataManager.Gold.ToString(), 0.5f);
    }

    private void UpdateGold(BigInteger _amount)
    {
        if (goldTweener != null && goldTweener.IsActive())
        {
            goldTweener.Kill();
            goldTweener = null;
        }

        GameManager.Instance.acquireMessageUI.Appear_AccuireBox(CurrencyType.Gold, _amount);
        goldTweener = DOTween.To(
            () => currencyPanelAccessor.GoldText.text,
            x => currencyPanelAccessor.GoldText.text = x,
            GameManager.Instance.userDataManager.Gold.ToString(), 0.5f);
    }
}