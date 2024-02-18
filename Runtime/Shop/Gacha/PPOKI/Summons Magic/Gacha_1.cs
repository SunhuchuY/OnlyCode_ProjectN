using UnityEngine;

namespace Shop
{
    namespace Gacha
    {
        namespace SummonsMagic
        {
            public class Gacha_1 : AdsBase
            {
                public Gacha_1() : base("SummonsMagicGacha1") { }

                protected override void Reward()
                {
                    base.Reward();
                    Gacha.Manager.Instance.StartGacha(GachaType.SummonsMagic, 1);
                }
            }
        }
    }
}

