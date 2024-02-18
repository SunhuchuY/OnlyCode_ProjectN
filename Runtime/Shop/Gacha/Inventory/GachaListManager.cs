using UnityEngine;
using UnityEngine.UI;

namespace Shop.Gacha.GachaListButtons
{
    public class GachaListManager : MonoBehaviour
    {
        public static GachaListManager Instance { get; private set; }

        [SerializeField] private Button OffButton;
        [SerializeField] private Button AllOpenButton;

        [SerializeField] private GameObject GachaList_UI;

        [SerializeField] private ImageManager imageManager;

        private int openCount = 0;
        private int openLen;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            ButtonAddListener();
        }

        private void ButtonAddListener()
        {
            OffButton.onClick.AddListener(() =>
            {
                GachaList_UI.SetActive(false);
            });

            AllOpenButton.onClick.AddListener(() =>
            {
                imageManager.AllOpen();

                OffButton.gameObject.SetActive(true);
                AllOpenButton.gameObject.SetActive(false);
            });
        }

        public void GachaListInitialize(int openLen)
        {
            OffButton.gameObject.SetActive(false);
            AllOpenButton.gameObject.SetActive(true);
            openCount = 0;

            this.openLen = openLen;
        }

        public void CardOpenEvent()
        {
            openCount++;

            if (openLen <= openCount)
            {
                OffButton.gameObject.SetActive(true);
                AllOpenButton.gameObject.SetActive(false);
            }
        }
    }
}
