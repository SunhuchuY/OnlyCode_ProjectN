using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


namespace Shop
{
    namespace Gacha
    {
        public enum GachaType
        {
            SummonsMagic,
            AttackMagic,
        }

        [System.Serializable]
        public abstract class DiamondsBase : MonoBehaviour
        {
            protected const string tableName = "userData";

            [SerializeField] private Button purchaseButton;
            // 구매에 필요한 다이아몬드
            protected int diamond { get; private set; }
            // 스킬 개수
            protected int gachaCount { get; private set; }
            protected GachaType gachaType { get; private set; }

            
            public DiamondsBase(int diamond, GachaType gachaType, int gachaCount)
            {
                this.diamond = diamond;
                this.gachaCount = gachaCount;   
                this.gachaType = gachaType; 
            }

            private void Awake()
            {
                ButtonAddListener();
            }

            private void ButtonAddListener()
            {
                purchaseButton.onClick.AddListener(() =>
                {
                    Purchase();
                });
            }

            protected void Purchase()
            {
                if (GameManager.Instance.userDataManager.userData.Diamond < diamond)
                {
                    GameManager.Instance.commonUI.ToastMessage("다이아가 부족합니다.");
                    return;
                }

                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Diamond, -1 * diamond);
                Shop.Gacha.Manager.Instance.StartGacha(gachaType, gachaCount);
            }

        }
    }
}

