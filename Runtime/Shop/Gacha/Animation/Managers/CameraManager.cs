using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Shop.Gacha.Animation
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private AnimationManager addListenerManager;
        public float originalOrthographicSize;

        private void Start()
        {
            originalOrthographicSize = Camera.main.orthographicSize;    
        }

        public void ZoomIn(float duration)
        {
            float originalOrthographicSize = Camera.main.orthographicSize;

            Camera.main.DOOrthoSize(4, duration).OnStart(() =>
            {
                originalOrthographicSize = Camera.main.orthographicSize;

            }).OnComplete(() =>
            {
                Camera.main.orthographicSize = originalOrthographicSize;
            });
        }

        public void Shake(float duration, float strength)
        {
            Camera.main.transform.DOShakePosition(duration, strength);
        }
    }
}
