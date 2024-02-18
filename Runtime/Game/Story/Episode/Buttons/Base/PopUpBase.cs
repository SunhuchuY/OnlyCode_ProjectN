using UnityEngine;
using UnityEngine.UI;

namespace Story
{
    public abstract class PopUpBase : MonoBehaviour
    {
        [SerializeField] private GameObject popUpObject;
        [SerializeField] private Button onPopUpButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        protected virtual void Awake()
        {
            ButtonAddListener();
        }

        private void ButtonAddListener()
        {
            onPopUpButton.onClick.AddListener(() =>
            {
                popUpObject.SetActive(true);
            });

            confirmButton.onClick.AddListener(() =>
            {
                Confirm();
            });

            cancelButton.onClick.AddListener(() =>
            {
                popUpObject.SetActive(false);
            });
        }

        protected abstract void Confirm();
    }
}
