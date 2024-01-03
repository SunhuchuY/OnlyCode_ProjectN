using UnityEngine;
using UnityEngine.UI;

public class EnterShop : MonoBehaviour
{
    [SerializeField] private Button enterShopButton;

    private void Awake()
    {
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        enterShopButton.onClick.AddListener(() =>
        {
            SceneManagement.Loader.Instance.ASync("Shop");
        });
    }
}
