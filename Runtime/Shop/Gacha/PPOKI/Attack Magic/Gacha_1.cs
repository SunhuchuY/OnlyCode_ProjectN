using BackEnd;
using System.Numerics;
using UnityEngine;

namespace Shop 
{
    namespace Gacha
    {
        namespace AttackMagic
        {
            public class Gacha_1 : AdsBase
            {
                public Gacha_1() : base("AttackMagicGacha1") { }

                protected override void Reward()
                {
                    base.Reward();
                    Gacha.Manager.Instance.StartGacha(GachaType.AttackMagic, 1);
                }
            }
        }
    }
}

