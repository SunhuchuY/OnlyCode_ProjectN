using System;
using System.Collections;
using UnityEngine;

namespace Shop
{
    namespace Gacha
    {
        namespace Animation
        {
            public class CircleManager : MonoBehaviour
            {
                [SerializeField] private CircleBase[] circles = new CircleBase[3];
                [SerializeField] public Color[] circleColors;

                public void OnRotate()
                {
                    foreach (var circle in circles)
                    {
                        circle.state = CircleState.Rotate;
                    }   
                }

                public void OnGlow(float duration)
                {
                    foreach (var circle in circles)
                    {
                        circle.SetGlow(duration, 4f);
                    }
                }

                public void OnRankColor()
                {
                    ChangeColor(circleColors[0], circleColors[1], circleColors[2]);
                }

                public void ChangeColor(Color color)
                {
                    foreach (var circle in circles)
                    {
                        circle.SetColor(color);
                    }
                }

                public void ChangeColor(Color color, Color color_2, Color color_3)
                {
                    circles[0].SetColor(color);
                    circles[1].SetColor(color_2);
                    circles[2].SetColor(color_3);
                }

                public void ChangeColor(int index, Color color)
                {
                    if (index < 0 || index > 3)
                    {
                        Debug.LogError(index + ": 인덱스는 반드시 0부터 2까지만 가능합니다.");
                        return;
                    }

                    circles[index].SetColor(color);
                }

                /// <summary>
                /// 점차 사라지는 애니메이션
                /// </summary>
                /// <param name="duration"></param>
                public void FadeOutCircles(float duration)
                {
                    StartCoroutine(FadeOutCirclesCoroutine(duration));
                }

                private IEnumerator FadeOutCirclesCoroutine(float duration)
                {
                    yield return new WaitForSeconds(duration);
                    foreach (var circle in circles)
                    {
                        circle.SetFade(0f, duration);
                    }
                }

                /// <summary>
                /// 점차 두꺼워지는 애니메이션
                /// </summary>
                /// <param name="duration"></param>
                public void EdgeCircles(float duration)
                {
                    foreach (var circle in circles)
                    {
                        circle.SetEdge(duration, 0.1f);
                    }
                }

                public void FadeInCircles()
                {
                    foreach (var circle in circles)
                    {
                        circle.SetFade(1f, 0.1f);
                    }
                }
            }   
        }
    }
}

