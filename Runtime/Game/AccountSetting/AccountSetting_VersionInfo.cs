using BackEnd;
using TMPro;
using UnityEngine;

/// <summary>
/// editor에서는 오류를 리턴합니다.
/// </summary>
public class AccountSetting_VersionInfo : MonoBehaviour, IAccountSetting_UI_Component
{
    [SerializeField] private TMP_Text versionText;

    public void Confirm()
    {
        GameManager.Instance.accountSettingsPanelUI.accessor.Popup.gameObject.SetActive(false);

    }
    private void Start()
    {
        var bro = Backend.Utils.GetLatestVersion();
        string version = bro.GetReturnValuetoJSON()["version"].ToString();

        versionText.text = version;
    }
}

