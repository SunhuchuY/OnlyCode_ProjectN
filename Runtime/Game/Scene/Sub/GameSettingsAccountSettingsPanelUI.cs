using UnityEngine;

public class GameSettingsAccountSettingsPanelUI : MonoBehaviour
{
    public GameSettingsAccountSettingsPanelAccessor accessor;
    private IAccountSetting_UI_Component current;
    
    private const string NaverCafeURL = "https://cafe.naver.com/indiedev";

    private void OnEnable()
    {
        accessor.ConfirmButton.onClick.AddListener(() => 
        {
            current?.Confirm();
        });

        accessor.AccountSettingExitButton.onClick.AddListener(() => 
        {
            accessor.gameObject.SetActive(false); 
        });

        accessor.PopupExitButton.onClick.AddListener(() =>
        {
            accessor.Popup.gameObject.SetActive(false);
            current = null;
        });


        accessor.DeleteAccountButton.onClick.AddListener(() =>
        {
            OpenContents();
            accessor.DeleteContents.gameObject.SetActive(true);
            accessor.TitleText.text = "계정 탈퇴";
            
            current = accessor.DeleteContents;
        });

        accessor.LogoutButton.onClick.AddListener(() => 
        {
            OpenContents();
            accessor.LogoutContents.gameObject.SetActive(true);
            accessor.TitleText.text = "로그 아웃";
            
            current = accessor.LogoutContents;
        });

        accessor.ContactButton.onClick.AddListener(() => 
        {
            Application.OpenURL(NaverCafeURL);
        });
        
        accessor.VersionButton.onClick.AddListener(() =>
        {
            OpenContents();
            accessor.VersionInfoContents.gameObject.SetActive(true);
            accessor.TitleText.text = "버전 정보";
            
            current = accessor.VersionInfoContents;
        });
        
        accessor.CouponButton.onClick.AddListener(() => 
        {
            OpenContents();
            accessor.CouponContents.gameObject.SetActive(true);
            accessor.TitleText.text = "쿠폰";
            accessor.CouponContents.gameObject.SetActive(true);

            current = accessor.CouponContents;
        });
    }   

    private void OpenContents()
    {
        DeActiveAllContents();
        accessor.Popup.gameObject.SetActive(true);
    }

    private void DeActiveAllContents()  
    {
        accessor.DeleteContents.gameObject.SetActive(false);
        accessor.CouponContents.gameObject.SetActive(false);
        accessor.LogoutContents.gameObject.SetActive(false);
        accessor.VersionInfoContents.gameObject.SetActive(false);
    }
}