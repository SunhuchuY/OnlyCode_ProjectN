using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    public class Loading : MonoBehaviour
    {
        public static Loading Instance; 
        
        private float _progress;
        public float progress
        {
            get
            {
                return _progress;
            }

            set
            {
                if (value > 100 || value < 0)
                    return;

                _progress = value;

                progressText.text = $"{value} %";
                Debug.Log(progressText.text);
                progressBarImage.transform.localScale = new Vector3(value / 100, progressBarImage.transform.localScale.y, progressBarImage.transform.localScale.z);
            }
        }

        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Image progressBarImage;


        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);  
        }
    }
}

