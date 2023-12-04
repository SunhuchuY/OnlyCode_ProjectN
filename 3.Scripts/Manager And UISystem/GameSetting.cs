using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{

    [SerializeField] private Image speedImage;
    [SerializeField] private Sprite[] speedSprites = new Sprite[3];
    [SerializeField] private GameObject BackGround_GameSetting;
    [SerializeField] private GameObject gameAccount_Tap, settingPanel;
    [SerializeField] private Slider backgroundMusic_Slider, effectMusic_InputField;
    
    public float fadeDuration = 1f;

    private Tween fadeTween;
    private float curSpeed = 1f;
    private bool isOnGameSetting = false;
    private bool isPause = false;



    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            BackGround_GameSetting.transform.GetChild(i).gameObject.SetActive(false);
        }

        BackGround_GameSetting.SetActive(false);
        settingPanel.SetActive(false);  
        gameAccount_Tap.SetActive(false);

        speedImage.sprite = speedSprites[0];
    }
    public void PlaySpeed_UpBtn()
    {
        switch (curSpeed)
        {
            case 1f: // 2배로
                speedImage.sprite = speedSprites[1];
                Time.timeScale = 1.5f;
                curSpeed = 2f;
                break;
            case 2f: // 3배로
                speedImage.sprite = speedSprites[2];
                Time.timeScale = 2f;
                curSpeed = 3f;

                break;
            case 3f: // 1배로
                speedImage.sprite = speedSprites[0];
                Time.timeScale = 1f;
                curSpeed = 1f;
                break;
        }
    }


    public void GameSetting_Button() // button
    {
        if (isOnGameSetting == true) // 창이 이미 열려있음
        {
            const int startIndex = 1;
            const int buttonofNum= 4;


            for (int i = startIndex; i < startIndex+4; i++)
            {
                BackGround_GameSetting.transform.GetChild(i).gameObject.SetActive(false);
            }

            BackGround_GameSetting.SetActive(false);

            isOnGameSetting = false;
        }
        else
        {
            BackGround_GameSetting.SetActive(true);
            StartCoroutine(OnGameSetting_Corutine());
            isOnGameSetting = true;
        }
    }

    IEnumerator OnGameSetting_Corutine()
    {
        float delay = 0.2f;
        Vector2 startVector = new Vector2(0.5f, 0.5f);

        const int startIndex = 0;
        const int buttonofNum = 5;

        for (int i = startIndex; i < startIndex+buttonofNum; i++)
        {
            BackGround_GameSetting.transform.GetChild(i).gameObject.SetActive(true);
            BackGround_GameSetting.transform.GetChild(i).localScale = startVector;
            BackGround_GameSetting.transform.GetChild(i).DOScale(Vector2.one, delay);
            yield return new WaitForSeconds(delay);
        }


    }

    private void FixedUpdate()
    {
        GameManager.Instance.audioManager.background_Audio.volume = backgroundMusic_Slider.value;
    }

    public void OnGameSetting_btn(BaseEventData data) // btn
    {
        gameAccount_Tap.SetActive(false);

        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;
    }
    public void OnGameAccountSetting_btn(BaseEventData data) // btn
    {

        gameAccount_Tap.SetActive(true);

        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;
    }

    public void OnGameAccountSetting_Exitbtn(BaseEventData data) // btn
    {

        gameAccount_Tap.SetActive(false);

        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;
    }

    public void OnGameSkillSetting_btn(BaseEventData data) // btn
    {
        gameAccount_Tap.SetActive(false);

        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;
    }

    public void GameSetting_Exit_Btn(BaseEventData data)
    {
        settingPanel.SetActive(false);
    }

    public void GameSetting_Entrance_Btn(BaseEventData data)
    {
        settingPanel.SetActive(true);
    }

}
