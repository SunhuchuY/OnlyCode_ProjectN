using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public abstract class GachaAnimationStateBase
    {
        public readonly float duration;
        public float currentTime { get; set; } = 0;

        protected AnimationManager animationManager;

        public GachaAnimationStateBase(AnimationManager manager, float duration)
        {
            animationManager = manager;
            this.duration = duration;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}
