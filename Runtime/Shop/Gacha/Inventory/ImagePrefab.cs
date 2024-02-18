using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.Gacha.GachaListButtons
{
    public class ImagePrefab : MonoBehaviour
    {
        // readonly Initialized Value
        private readonly Vector3 startRotation = new Vector3(0,180,0);
        private readonly Vector3 endRotation = new Vector3(0,0,0);
        
        // Turn
        private readonly float turnDuration = 1f;
        private Button turnButton;

        // Transforms
        [SerializeField] private Transform skillTf;

        // Images
        [SerializeField] private Image overrideImage;
        [SerializeField] private Image OutlineImage;
        public Image skillImage;

        public bool isAleadyOpen { get; private set; }

        public void Initialize()
        {
            turnButton = overrideImage.GetComponent<Button>();    
            ButtonAddListener();
            
            Material overrideImageMaterial = Material.Instantiate(overrideImage.material);
            Material outlineImageMaterial = Material.Instantiate(OutlineImage.material);

            overrideImage.material = overrideImageMaterial;
            OutlineImage.material = outlineImageMaterial;

        }

        private void OnEnable()
        {
            isAleadyOpen = false;
            SetActive(true, false, false);
            skillImage.transform.rotation = Quaternion.Euler(startRotation);  
        }

        private void ButtonAddListener()
        {
            turnButton.onClick.AddListener(() =>
            {
                Open();
            });
        }

        private void SetActive(bool overrideImageActive, bool skillImageActive, bool outlineActive)
        {
            overrideImage.gameObject.SetActive(overrideImageActive);
            OutlineImage.gameObject.SetActive(outlineActive);
            skillImage.gameObject.SetActive(skillImageActive);
        }

        public void SetOverrideImageColor(Color color)
        {
            overrideImage.material.SetColor("_GlowColor", color);  
            OutlineImage.material.SetColor("_GlowColor", color);  
        }

        public void Open()
        {
            isAleadyOpen = true;

            SetActive(false, true, true);
            GachaListManager.Instance?.CardOpenEvent();
            skillTf.DORotate(endRotation, turnDuration)
                .From(startRotation)
                .OnStart(() =>
                {
                    skillImage.transform.rotation = Quaternion.Euler(endRotation);
                });
        }
    }
}
