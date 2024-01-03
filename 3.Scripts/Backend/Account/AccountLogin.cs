using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountLogin : MonoBehaviour
{
    [SerializeField] private GameObject customLoginBox; 

    [SerializeField] private Button customLoginButton;
    [SerializeField] private Button googleplaygamesLoginButton;

    
    private void Awake()
    {
        customLoginButton.onClick.AddListener(() =>
        {
            customLoginBox.SetActive(true);


        });

        googleplaygamesLoginButton.onClick.AddListener(() =>
        {
            Authenticate.GooglePlay.Account.Login();

        });
    }
}