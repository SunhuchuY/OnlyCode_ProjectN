using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Shop.Gacha.Animation
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private GachaAnimationUI_GameObjects _chaAnimationUI_GameObjects;
        public GachaAnimationUI_GameObjects chaAnimationUI_GameObjects => _chaAnimationUI_GameObjects;

        [SerializeField] private GachaAnimationUI_Buttons _gachaAnimationUI_Buttons;
        public GachaAnimationUI_Buttons gachaAnimationUI_Buttons => _gachaAnimationUI_Buttons;  

        [SerializeField] private CircleManager _circleManager;
        public CircleManager circleManager => _circleManager;

        [SerializeField] private CameraManager _cameraManager;
        public CameraManager cameraManager => _cameraManager;

        // state pattern
        private Queue<GachaAnimationStateBase> stateQueue = new Queue<GachaAnimationStateBase>();
        private GachaAnimationStateBase currentState;


        private void OnEnable()
        {
            chaAnimationUI_GameObjects.gachaList_UI.SetActive(false);
        }

        private void Update()
        {
            if (currentState == null)
            {
                if (stateQueue.Count > 0)
                {
                    SwitchToState(stateQueue.Dequeue());
                }
                return;
            }

            currentState.currentTime += Time.deltaTime;

            if (currentState.currentTime > currentState.duration)
            {
                if (stateQueue.Count > 0)
                {
                    SwitchToState(stateQueue.Dequeue());
                }
                else
                {
                    currentState.Exit();
                    currentState = null;
                }
            }
        }

        public void SwitchToState(GachaAnimationStateBase newState)
        {
            // existed
            if (currentState != null)
            {
                currentState.Exit();
            }

            // new 
            if (newState != null)
            {
                newState.Enter();
            }

            currentState = newState;
        }

        // execute: 가챠를 시작하기 위한 버튼이 나오기 전 애니메이션을 실행합니다.
        public void StartButtonWithFade(float duration)
        {
            stateQueue.Enqueue(new StartButtonWithFadeState(this, duration));
        }

        // execute: 가챠 리스트를 불러오는 애니메이션을 실행합니다.
        public void OnGachaList(float duration)
        {
            float fadeOutTime = 1f;
            float half = duration / 2;

            stateQueue.Enqueue(new EdgeCircleState(this, half));
            stateQueue.Enqueue(new ZoomInState(this, half));
            stateQueue.Enqueue(new FadeOutState(this, fadeOutTime));
            stateQueue.Enqueue(new GachaEndState(this, 0.1f));
        }
    }
}
