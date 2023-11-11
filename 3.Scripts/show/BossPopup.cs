using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPopup : MonoBehaviour
{
    [SerializeField] GameObject bossPopUpObject;

    private void Start()
    {
        bossPopUpObject.SetActive(false);
    }

    public void BossPopupOff()
    {
        bossPopUpObject.SetActive(false);   
    }
}
