using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomLogin : MonoBehaviour
{
    [SerializeField] private GameObject findIDBox;
    [SerializeField] private GameObject resetPasswordBox;
    [SerializeField] private GameObject signUpBox;

    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private TMP_Text informationText;

    [SerializeField] private Button loginButton;
    [SerializeField] private Button onFindIDBoxButton;
    [SerializeField] private Button onResetPasswordBoxButton;
    [SerializeField] private Button onSignUpBoxButton;


    private void Awake()
    {
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        loginButton?.onClick.AddListener(() =>
        {
            bool isLogin = Authenticate.Custom.Account.Login(idInput.text, passwordInput.text);

            if (isLogin)
            {
                ChangeInformationText("로그인 성공", Color.green);
            }
            else
            {
                passwordInput.text = "";
                ChangeInformationText("아이디나 비밀번호를 찾을 수 없습니다.", Color.red);
            }
        });


        onSignUpBoxButton?.onClick.AddListener(() =>
        {
            signUpBox.SetActive(true);
        });


        onFindIDBoxButton?.onClick.AddListener(() =>
        {
            findIDBox.SetActive(true);
        });

        onResetPasswordBoxButton?.onClick.AddListener(() =>
        {
            resetPasswordBox.SetActive(true);
        });
    }

    private void ChangeInformationText(string text, Color color)
    {
        informationText.text = text;
        informationText.color = color;
    }
}