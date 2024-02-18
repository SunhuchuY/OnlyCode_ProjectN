using UnityEngine;
using UnityEngine.UI;

public class AccountSetting_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _popup;
    [SerializeField] private Button _enter;
    [SerializeField] private Button _exit;

    private void Awake()
    {
        _enter.onClick.AddListener(() =>
        {
            _popup.SetActive(true);
        });

        _exit.onClick.AddListener(() =>
        {
            _popup.SetActive(false);
        });
    }
}