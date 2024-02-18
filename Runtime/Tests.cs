using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour
{
    [Header("커스텀로그인")]
    [SerializeField] private GameObject customLoginBox;
    [SerializeField] private Button customLoginButton;

    [SerializeField] private Button EnterGameButton;

    private void Awake()
    {
#if UNITY_EDITOR
        customLoginButton.onClick.AddListener(() =>
        {
            customLoginBox.SetActive(true);
        });

        EnterGameButton.onClick.AddListener(() => 
        {
            SceneLoader.LoadSceneAsync(SceneLoader.SceneType.mainTest);
        });

        customLoginButton.gameObject.SetActive(true);
        EnterGameButton.gameObject.SetActive(true);
#endif
    }
}
