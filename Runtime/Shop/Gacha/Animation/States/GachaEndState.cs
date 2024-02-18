using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class GachaEndState : GachaAnimationStateBase
    {
        public GachaEndState(AnimationManager animationManager, float duration) : base(animationManager, duration) { }

        public override void Enter()
        {
            animationManager.chaAnimationUI_GameObjects.gachaBox_World.SetActive(false);
        }

        public override void Exit()
        {
        }
    }
}