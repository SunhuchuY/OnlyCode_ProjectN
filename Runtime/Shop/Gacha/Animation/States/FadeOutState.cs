using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class FadeOutState : GachaAnimationStateBase
    {
        public FadeOutState(AnimationManager animationManager, float duration) : base(animationManager, duration) { }

        public override void Enter()
        {
            animationManager.circleManager.FadeOutCircles(duration / 2);
            animationManager.chaAnimationUI_GameObjects.gachaList_UI.SetActive(true);
            animationManager.chaAnimationUI_GameObjects.gachaBox_UI.SetActive(false);
        }

        public override void Exit()
        {

        }
    }
}
