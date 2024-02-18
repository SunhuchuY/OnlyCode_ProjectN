using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.Gacha.Animation
{
    public class GachaAnimationUI_Buttons : MonoBehaviour
    {
        [SerializeField] private Button _gachaStartButton;
        public Button gachaStartButton => _gachaStartButton;

        [SerializeField] private CircleManager circleManager;
        [SerializeField] private GachaAnimationUI_Events gachaAnimationUI_Events;



        private void Awake()
        {
            ButtonAddListener();
        }

        private void ButtonAddListener()
        {
            gachaStartButton.onClick.AddListener(() =>
            {
                circleManager.OnRotate();
                circleManager.OnRankColor();
                gachaAnimationUI_Events.CachaListEventHandler(3.5f);
                gachaStartButton.gameObject.SetActive(false);
            });
        }
    }


}
