using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomSignUp : MonoBehaviour
{
    [SerializeField] private GameObject signUpBox;

    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField passwordReCheckInput;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private TMP_InputField emailInput;

    [SerializeField] private TMP_Text informationText;

    [SerializeField] private Button signUpButton;
    [SerializeField] private Button offSignUpBoxButton;

    private void Awake()
    {
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        signUpButton?.onClick.AddListener(() =>
        {
            if (passwordInput.text != passwordReCheckInput.text)
            {
                ChangeInformationText("비밀번호가 일치하지 않습니다.", Color.red);
                return;
            }

            bool isSignUp = Authenticate.Custom.Account.SignUp(idInput.text, passwordInput.text, nicknameInput.text, emailInput.text);

            if (isSignUp)
            {
#if UNITY_EDITOR
                Debug.Log("회원가입이 성공적으로 완료 되었습니다.");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("회원가입 에러");
#endif
            }
        });


        offSignUpBoxButton?.onClick.AddListener(() =>
        {
            signUpBox.SetActive(false);
        });
    }

    private void ChangeInformationText(string text, Color color)
    {
        informationText.text = text;
        informationText.color = color;
    }
}
