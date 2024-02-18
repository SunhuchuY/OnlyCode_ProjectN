using System;
using System.Numerics;
using UnityEngine;

namespace Shop
{
    namespace Daily
    {
        public class FreeMagicStone : AdsBase
        {
            private const int amount = 5_000;

            public FreeMagicStone() : base("FreeMagicStone") { }

            protected override void Reward()
            {
                base.Reward();
                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone, amount);
            }
        }
    }
}