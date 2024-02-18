using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class EdgeCircleState : GachaAnimationStateBase
    {
        public EdgeCircleState(AnimationManager animationManager, float duration) : base(animationManager, duration) { }

        public override void Enter()
        {
            animationManager.circleManager.EdgeCircles(duration);
        }

        public override void Exit()
        {
        }
    }
}