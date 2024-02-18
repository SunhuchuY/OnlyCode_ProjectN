using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    namespace Gacha
    {
        namespace Animation
        {
            public class FaadInOut : MonoBehaviour
            {
                [SerializeField] private Image gachaStartBackgroundImage;
                [SerializeField] private Image gachaListBackgroundImage;

                public void GachaStartFadeOut(float duration)
                {
                    Color next = gachaStartBackgroundImage.color;
                    next.a = 0;   

                    gachaStartBackgroundImage.color = next;  
                    gachaStartBackgroundImage.DOFade(1, duration);
                }

                public void GachaListFadeOut(float duration)
                {
                    Color next = gachaListBackgroundImage.color;
                    next.a = 0;

                    gachaListBackgroundImage.color = next;
                    gachaListBackgroundImage.DOFade(1, duration);
                }
            }
        }
    }
}

