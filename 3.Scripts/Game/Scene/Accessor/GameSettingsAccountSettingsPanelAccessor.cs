using UnityEngine;
using UnityEngine.UI;

public class GameSettingsAccountSettingsPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button backgroundQuitButton;
    [SerializeField] private Button changeAccountButton;
    [SerializeField] private Button resetAccountButton;
    [SerializeField] private Button deleteAccountButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button contactButton;
    [SerializeField] private Button versionButton;
    [SerializeField] private Button couponButton;

    public Button BackgroundQuitButton => backgroundQuitButton;
    public Button ChangeAccountButton => changeAccountButton;
    public Button ResetAccountButton => resetAccountButton;
    public Button DeleteAccountButton => deleteAccountButton;
    public Button LogoutButton => logoutButton;
    public Button ContactButton => contactButton;
    public Button VersionButton => versionButton;
    public Button CouponButton => couponButton;
}