using System.Numerics;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.Gacha
{
    public class GachaPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(() =>
            {
                GameManager.Instance.gachaTf.gameObject.SetActive(false);
            });

            GameManager.Instance.userDataManager.userData
                .ObserveEveryValueChanged(x => x.Diamond)
                .Subscribe(DiamondTextUpdate);
            DiamondTextUpdate(GameManager.Instance.userDataManager.userData.Diamond);
        }

        private void DiamondTextUpdate(BigInteger _value)
        {
            text.text = CurrencyHelper.ToCurrencyString(_value);
        }
    }
}