using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class GachaAnimationUI_Events : MonoBehaviour
    {
        [SerializeField] private AnimationManager addListenerManager;
        [SerializeField] private CircleManager circleManager;
        [SerializeField] private FaadInOut fadeInOut;

        private event Action<float> FadeoutStartEvent;
        private event Action<float> OnGachaListEvent;


        private void Awake()
        {
            EventAddListener();
        }

        private void EventAddListener()
        {
            FadeoutStartEvent += fadeInOut.GachaStartFadeOut;
            FadeoutStartEvent += addListenerManager.StartButtonWithFade;

            OnGachaListEvent += circleManager.OnGlow;
            OnGachaListEvent += addListenerManager.OnGachaList;
        }

        #region EventHendler
        // execute: 가차 리스트를 불러오는 핸들러
        public void FadeoutStartEventHandler(float duration)
        {
            FadeoutStartEvent?.Invoke(duration);
        }

        // execute: 가차 리스트를 불러오는 핸들러
        public void CachaListEventHandler(float duration)
        {
            OnGachaListEvent?.Invoke(duration);
        }
        #endregion
    }
}