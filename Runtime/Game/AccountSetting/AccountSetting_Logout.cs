using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccountSetting_Logout : MonoBehaviour, IAccountSetting_UI_Component
{
    const float WAIT_GAMESCENE = 2f;
    const string info = "로그아웃 되었습니다.\n타이틀화면으로 돌아갑니다.";
    
    public void Confirm()
    {
        var bro = BackEnd.Backend.BMember.Logout();

        if (bro.IsSuccess())
        {
            StartCoroutine(DelayLogout());
        }
    }

    IEnumerator DelayLogout()
    {
        GameManager.Instance.commonUI.ToastMessage(info);
        yield return new WaitForSeconds(WAIT_GAMESCENE);
        SceneLoader.LoadSceneAsync(SceneLoader.SceneType.Menu);
    }
}