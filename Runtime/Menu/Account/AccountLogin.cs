using BackEnd;
using System.Collections;
using UnityEngine;

public class AccountLogin : MonoBehaviour
{
    [SerializeField] private AccountLoginAccessor accessor;

    private void Awake()
    {
        accessor.gameObject.SetActive(true);

        accessor.GoogleplaygamesLoginButton.onClick.AddListener(() =>
        {
            TheBackend.ToolKit.GoogleLogin.Android.GoogleLogin(GoogleLoginCallback);

            if (Backend.IsLogin)
            {
                accessor.GoogleplaygamesLoginButton.gameObject.SetActive(false);

                if (Backend.UserNickName == string.Empty)
                {
                    accessor.NickNamePanel.gameObject.SetActive(true);
                }
                else
                {
                    EnterInGame();
                }
            }
        });

        accessor.NickNameApplyButton.onClick.AddListener(() =>
        {
            string input = accessor.NickNameInput.text;

            if (input != string.Empty)
            {
                var bro = Backend.BMember.CreateNickname(input);
                EnterInGame();
            }
        }); 
    }

    private void EnterInGame()
    {
        SceneLoader.LoadSceneAsync(SceneLoader.SceneType.mainTest);
    }
    private void GoogleLoginCallback(bool isSuccess, string errorMessage, string token)
    {
        if (isSuccess == false)
        {
            Debug.LogError(errorMessage);
            return;
        }

        var bro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
        Debug.Log("페데레이션 로그인 결과 : " + bro);

        if (bro.GetStatusCode() == "201")
        {
            Backend.BMember.CreateNickname(string.Empty);
        }
    }
}