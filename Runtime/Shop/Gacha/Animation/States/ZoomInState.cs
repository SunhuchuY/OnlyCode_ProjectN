using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class ZoomInState : GachaAnimationStateBase
    {
        public ZoomInState(AnimationManager animationManager, float duration) : base(animationManager, duration) { }

        public override void Enter()
        {
            animationManager.cameraManager.Shake(1f, 1f);
            animationManager.cameraManager.ZoomIn(duration);
        }

        public override void Exit()
        {
        }
    }
}
