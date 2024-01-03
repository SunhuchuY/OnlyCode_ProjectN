using UnityEngine;

public class GameSettingsAccountSettingsPanelUI : MonoBehaviour
{
    public GameSettingsAccountSettingsPanelAccessor accessor;

    private void OnEnable()
    {
        accessor.BackgroundQuitButton.onClick.AddListener(() => accessor.gameObject.SetActive(false));
        accessor.ChangeAccountButton.onClick.AddListener(() => { Debug.Log("ChangeAccountButton Clicked!"); });
        accessor.ResetAccountButton.onClick.AddListener(() => { Debug.Log("ResetAccountButton Clicked!"); });
        accessor.DeleteAccountButton.onClick.AddListener(() => { Debug.Log("DeleteAccountButton Clicked!"); });
        accessor.LogoutButton.onClick.AddListener(() => { Debug.Log("LogoutButton Clicked!"); });
        accessor.ContactButton.onClick.AddListener(() => { Debug.Log("ContactButton Clicked!"); });
        accessor.VersionButton.onClick.AddListener(() => { Debug.Log("VersionButton Clicked!"); });
        accessor.CouponButton.onClick.AddListener(() => { Debug.Log("CouponButton Clicked!"); });

        accessor.gameObject.SetActive(false);
    }
}