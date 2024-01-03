using UnityEngine;
using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace Authenticate
{
    namespace GooglePlay
    {
        public class Account
        {
            public static void Login()
            {
                // gpgs v10.14
                // gpgs plugin 설정
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                        .Builder()
                    .RequestServerAuthCode(false)
                    .RequestEmail()
                    .RequestIdToken()
                    .Build();

                //커스텀 된 정보로 GPGS 초기화
                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.DebugLogEnabled = true;

                PlayGamesPlatform.Instance.Authenticate((_success, _str) =>
                {
                    Debug.Log(_str);

                    if (_success)
                        OnGpgsLoginSuccess();
                    else
                    {
                        Debug.Log("GPGS Login Failed!");
                    }
                });
            }

            public static void AttemptLoginToBackend(string _token)
            {
                // 뒤끝 backend에 로그인을 시도합니다.
                Backend.BMember.AuthorizeFederation(_token, FederationType.Google, "GPGS로 가입함", bro =>
                {
                    if (bro.IsSuccess())
                    {
#if UNITY_EDITOR
                        Debug.Log("Backend Login Success!");
#endif
                        OnBackendLoginSuccess(bro);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log($"error code: {bro.GetErrorCode()}");
                        Debug.Log($"message: {bro.GetMessage()}");
                        Debug.Log($"return value: {bro.GetReturnValue()}");
                        Debug.Log("Backend Login Failed!");
#endif
                    }
                });
            }

            public static void OnGpgsLoginSuccess()
            {
#if UNITY_EDITOR
                Debug.Log("GPGS Login Success!");
#endif

                // gpgs v10.14
                string _token = PlayGamesPlatform.Instance.GetIdToken();
                AttemptLoginToBackend(_token);
#if UNITY_EDITOR

                Debug.Log($"tokens: {_token}");
#endif
            }

            public static void OnBackendLoginSuccess(BackendReturnObject _bro)
            {
#if UNITY_EDITOR
                Debug.Log("Backend Login Success!");
#endif
            }
        }

    }

}