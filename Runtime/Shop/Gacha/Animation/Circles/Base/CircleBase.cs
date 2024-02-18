using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    namespace Gacha
    {
        namespace Animation
        {
            public enum CircleState
            {
                Idle, 
                Rotate
            }

            [System.Serializable]
            public abstract class CircleBase : MonoBehaviour
            {
                public const float start_LightingAmount = 0.5f;
                public const float start_EdgeAmount = 0.9f;
                private readonly Vector3 rotateDirection  = new Vector3(0, 0, 1);

                [SerializeField] private float rotateSpeed = 5f;
                [HideInInspector] public CircleState state { get; set; }
                [SerializeField] private SpriteRenderer spriteRenderer;


                private void OnEnable()
                {
                    state = CircleState.Idle; 
                    spriteRenderer.material.SetFloat("_LightingAmount", start_LightingAmount);
                    spriteRenderer.material.SetFloat("_EdgeAmount", start_EdgeAmount);
                }   

                private void Update()
                {
                    if(state == CircleState.Rotate)
                    {
                        transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);
                    }
                }

                public void SetColor(Color color)
                {
                    spriteRenderer.material.SetColor("_GlowColor", color);
                }

                public void SetFade(float end, float duration)
                {
                    spriteRenderer.DOFade(end, duration);
                }

                public void SetGlow(float duration, float end, float start = start_LightingAmount)
                {
                    spriteRenderer.material.DOFloat(end, "_LightingAmount", duration)
                        .From(start);
                }

                public void SetEdge(float duration, float end, float start = start_EdgeAmount)
                {
                    spriteRenderer.material.DOFloat(end, "_EdgeAmount", duration)
                        .From(start);
                }
            }
        }
    }
}

