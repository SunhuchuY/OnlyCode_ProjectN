using System.Collections;
using UniRx;
using UnityEngine;

public class LevelUpPopupUI : MonoBehaviour
{
    [SerializeField] private LevelUpPopupUIAccessor accessor;

    private void Start()
    {
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CurrentLevel)
            .Skip(1) // Skip the first emission
            .Subscribe(_ => OnPopUp());
    }

    private void OnPopUp()
    {
        int currentLevel = GameManager.Instance.userDataManager.userData.CurrentLevel;
        int beforeLevel = currentLevel - 1;
        int currentAtk = GameManager.Instance.playerScript.Stats["Attack"].CurrentValueInt;

        accessor.ChangeLevelText.text = $"Lv.{beforeLevel} -> Lv.{currentLevel}";
        accessor.ChangeAtkText.text = $"{CurrencyHelper.ToCurrencyString(currentAtk * beforeLevel)} -> {CurrencyHelper.ToCurrencyString(currentAtk * currentLevel)}";

        accessor.gameObject.SetActive(true);
        accessor.Animation.Play();
    }
}