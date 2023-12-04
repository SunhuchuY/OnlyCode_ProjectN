using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text diaText;

    private void Start()
    {
        shopPanel.SetActive(false); 
    }

    private void FixedUpdate()
    {
        diaText.text = $"{GameManager.Instance.uIManager.soulFragment}";
    }

    public void ShopIn_Funtion(BaseEventData data)
    {
        shopPanel.SetActive(true);
    }

    public void ShopExit_Funtion(BaseEventData data)
    {
        shopPanel.SetActive(false);
    }
    
}
