using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountLoginAccessor : MonoBehaviour
{
    [SerializeField] private Button googleplaygamesLoginButton;
    [SerializeField] private RectTransform nickNamePanel;
    [SerializeField] private Button nickNameApplyButton;
    [SerializeField] private TMP_InputField nickNameInput;

    [SerializeField] private Button gameQuitConfirmButton;
    [SerializeField] private RectTransform messagePopup;

    public Button GoogleplaygamesLoginButton => googleplaygamesLoginButton;
    public RectTransform NickNamePanel => nickNamePanel;
    public Button NickNameApplyButton => nickNameApplyButton;
    public TMP_InputField NickNameInput => nickNameInput;

    public Button GameQuitConfirmButton => gameQuitConfirmButton;
    public RectTransform MessagePopup => messagePopup;
}
