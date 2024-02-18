using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FindCustomAccount : MonoBehaviour
{
    [SerializeField] private GameObject findIDBox;
    [SerializeField] private GameObject resetPasswordBox;

    [SerializeField] private TMP_Text resetPasswordInformationText;
    [SerializeField] private TMP_Text findIDInformationText;

    [SerializeField] private Button findIDButton;
    [SerializeField] private Button resetPasswordButton;
    [SerializeField] private Button offResetPasswordBoxButton;
    [SerializeField] private Button offFindIDBoxButton;

    // 아이디를 찾을 때 보내는 이메일 
    [SerializeField] private TMP_InputField emailOfFindIDInput;
    // 비밀번호를 리셋 할 때 보내는 이메일
    [SerializeField] private TMP_InputField emailOfResetPasswordInput;
    // 비밀번호를 리셋 할 때 쓰는 ID
    [SerializeField] private TMP_InputField idOfResetPasswordInput;


    private void Awake()
    {
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        findIDButton.onClick.AddListener(() =>
        {
            string email = emailOfFindIDInput.text;
            bool isSendEmail = Authenticate.Find.FindID(email);

            if (isSendEmail)
            {
                ChangeInformationResetPassword("작성하신 이메일로 ID를 전송 했습니다.", Color.green);
            }
            else
            {
                ChangeInformationResetPassword("등록하신 이메일이 없습니다.", Color.red);
            }
        });


        resetPasswordButton.onClick.AddListener(() =>
        {
            string email = emailOfResetPasswordInput.text;
            string id = emailOfResetPasswordInput.text;
            bool isSendEmail = Authenticate.Reset.Password(id, email);

            if (isSendEmail)
            {
                ChangeInformationFindID("작성하신 이메일로 ID를 전송 했습니다.", Color.green);
            }
            else
            {
                ChangeInformationFindID("등록하신 이메일이 없습니다.", Color.red);
            }
        });


        offFindIDBoxButton?.onClick.AddListener(() =>
        {
            findIDBox.SetActive(false);
        });


        offResetPasswordBoxButton?.onClick.AddListener(() =>
        {
            resetPasswordBox.SetActive(false);
        });
    }

    private void ChangeInformationResetPassword(string text, Color color)
    {
        resetPasswordInformationText.text = text;
        resetPasswordInformationText.color = color;
    }
    
    private void ChangeInformationFindID(string text, Color color)
    {
        findIDInformationText.text = text;
        findIDInformationText.color = color;
    }
}