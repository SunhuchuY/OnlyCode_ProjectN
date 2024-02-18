using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountLoginAccessor : MonoBehaviour
{
    [SerializeField] private Button googleplaygamesLoginButton;
    [SerializeField] private RectTransform nickNamePanel;
    [SerializeField] private Button nickNameApplyButton;
    [SerializeField] private TMP_InputField nickNameInput;

    public Button GoogleplaygamesLoginButton => googleplaygamesLoginButton;

    public RectTransform NickNamePanel => nickNamePanel;
    public Button NickNameApplyButton => nickNameApplyButton;
    public TMP_InputField NickNameInput => nickNameInput;
}
