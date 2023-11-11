using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject shopPanel;
    [SerializeField] TMP_Text diaText;

    private void Start()
    {
        shopPanel.SetActive(false); 
    }

    private void FixedUpdate()
    {
        diaText.text = $"{GameManager.uIManager.soulFragment}";
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
