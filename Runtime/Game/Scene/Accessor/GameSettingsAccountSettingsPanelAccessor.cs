using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsAccountSettingsPanelAccessor : MonoBehaviour
{
    [SerializeField] private Button deleteAccountButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button contactButton;
    [SerializeField] private Button versionButton;
    [SerializeField] private Button couponButton;

    [SerializeField] private Button confirmButton;
    
    [SerializeField] private Button popupExitButton;
    [SerializeField] private Button accountSettingExitButton;

    [SerializeField] private TMP_Text titleText;

    [SerializeField] private RectTransform popup;

    [SerializeField] private AccountSetting_Delete deleteContents;
    [SerializeField] private AccountSetting_Logout logoutContents;
    [SerializeField] private AccountSetting_Coupon couponContents;
    [SerializeField] private AccountSetting_VersionInfo versionInfoContents;

    public Button DeleteAccountButton => deleteAccountButton;
    public Button LogoutButton => logoutButton;
    public Button ContactButton => contactButton;
    public Button VersionButton => versionButton;
    public Button CouponButton => couponButton;

    public Button ConfirmButton => confirmButton;

    public Button PopupExitButton => popupExitButton;
    public Button AccountSettingExitButton => accountSettingExitButton;

    public TMP_Text TitleText => titleText;

    public RectTransform Popup => popup;

    public AccountSetting_Delete DeleteContents => deleteContents;
    public AccountSetting_Logout LogoutContents => logoutContents;
    public AccountSetting_Coupon CouponContents => couponContents;
    public AccountSetting_VersionInfo VersionInfoContents => versionInfoContents;
}