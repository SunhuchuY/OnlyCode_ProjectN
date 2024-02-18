using BackEnd;
using System.Collections;
using TMPro;
using UnityEngine;

public class AccountSetting_Delete : MonoBehaviour, IAccountSetting_UI_Component
{
    const float DELAY_SECONDS = 2f;

    const string INFO = "정상적으로 탈퇴 처리 되었습니다.";

    public void Confirm()
    {
        var bro = Backend.BMember.WithdrawAccount(24 * 7);

        if (bro.IsSuccess())
        {
            StartCoroutine(DelayDelete());
        }
    }

    IEnumerator DelayDelete()
    {
        GameManager.Instance.commonUI.ToastMessage(INFO);
        yield return new WaitForSeconds(DELAY_SECONDS);
        SceneLoader.LoadSceneAsync(SceneLoader.SceneType.Menu);
    }
}