using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Shop.Gacha.Animation
{
    public class StartButtonWithFadeState : GachaAnimationStateBase
    {
        public StartButtonWithFadeState(AnimationManager animationManager, float duration) : base(animationManager, duration) { }

        public override void Enter()
        {
            animationManager.chaAnimationUI_GameObjects.gachaBox_UI.SetActive(true);
            animationManager.circleManager.FadeInCircles();
        }

        public override void Exit()
        {
            animationManager.gachaAnimationUI_Buttons.gachaStartButton.gameObject.SetActive(true);
            animationManager.chaAnimationUI_GameObjects.gachaBox_World.SetActive(true);
        }
    }
}
