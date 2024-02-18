using GoogleMobileAds.Api;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    namespace Daily
    {
        public class FreeDiamond : AdsBase 
        {
            public FreeDiamond() : base("FreeDiamond") { }

            protected override void Reward()
            {
                base.Reward();
                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Diamond, 100);
            }
        }  
    }
}