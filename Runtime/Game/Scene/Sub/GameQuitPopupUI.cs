using UnityEngine;
using UnityEngine.UI;

public class GameQuitPopupUI : MonoBehaviour
{
    [SerializeField] private RectTransform quitPopup;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

#if UNITY_ANDROID
    private void Start()
    {
        confirmButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        cancelButton.onClick.AddListener(() =>
        {
            quitPopup.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitPopup.gameObject.SetActive(true);
        }
    }
#endif
}