using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] GameObject WeekMissionObj, DailyMissionObj; // mission parents
    [SerializeField] Quest dailyJoinQueset, weekJoinQueset;

    const int DailyMax = 1, WeekMax = 1;

    private void Start()
    {
        LoadPlayerData();
        LoadPlayerData_week();

        Debug.Log($" week : {savedDateTime_Daily}");
        Debug.Log($" week : {savedDateTime_Week}");

        DailyJoin_Reward();
        WeekJoin_Reward();
    }



    // 일일 보상

    DateTime savedDateTime_Daily;

    private void SavePlayerData()
    {
        string savedDateTime_DailyString = savedDateTime_Daily.ToString();
        PlayerPrefs.SetString("savedDateTime_Daily", savedDateTime_DailyString);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        string loadedDateTime_DailyString = PlayerPrefs.GetString("savedDateTime_Daily", string.Empty);

        if (!string.IsNullOrEmpty(loadedDateTime_DailyString))
        {
            DateTime loadedDateTime_Daily = DateTime.Parse(loadedDateTime_DailyString);
            savedDateTime_Daily = loadedDateTime_Daily;
        }
    }

    // 주간 보상
    DateTime savedDateTime_Week;
    private void SavePlayerData_week()
    {
        string savedDateTime_DailyString = savedDateTime_Week.ToString();
        PlayerPrefs.SetString("savedDateTime_Week", savedDateTime_DailyString);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData_week()
    {
        string loadedDateTime_WeekString = PlayerPrefs.GetString("savedDateTime_Week", string.Empty);

        if (!string.IsNullOrEmpty(loadedDateTime_WeekString))
        {
            DateTime loadedDateTime_Week = DateTime.Parse(loadedDateTime_WeekString);
            savedDateTime_Week = loadedDateTime_Week;
        }
    }






    // 일일 미션 버튼
    public void DailyMissionOn()
    {
        WeekMissionObj.SetActive(false);
        DailyMissionObj.SetActive(true);
    }

    // 일일 접속 보상
    public void DailyJoin_Reward(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;

        DateTime nowDate = DateTime.Now;

        if (nowDate.Date != savedDateTime_Daily.Date)
        {
            Debug.Log("Reward");
            savedDateTime_Daily = nowDate;
            selectediconObj.transform.parent.GetComponent<Quest>().Completed();

            SavePlayerData();
        }
        else
        {
            selectediconObj.transform.parent.GetComponent<Quest>().NotCompleted();
            Debug.Log("Already Reward");
        }
    }

    public void DailyJoin_Reward()
    {
        DateTime nowDate = DateTime.Now;

        if (nowDate.Date != savedDateTime_Daily.Date)
        {
            Debug.Log("Reward");
            savedDateTime_Daily = nowDate;
            dailyJoinQueset.Completed();

            SavePlayerData();
        }
        else
        {
            dailyJoinQueset.NotCompleted();
            Debug.Log("Already Reward");
        }
    }



    // 주간 미션 버튼
    public void WeekMissionIn()
    {
        WeekMissionObj.SetActive(true);
        DailyMissionObj.SetActive(false);
    }
    public void WeekJoin_Reward(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        GameObject selectediconObj = pointerData.pointerPress;

        DateTime nowDate = DateTime.Now;

        TimeSpan difference = nowDate - savedDateTime_Week;
        if (difference.TotalDays > 7)
        {
            Debug.Log("Reward");

            savedDateTime_Daily = nowDate;
            selectediconObj.transform.parent.GetComponent<Quest>().Completed();
            SavePlayerData_week();
        }
        else
        {
            selectediconObj.transform.parent.GetComponent<Quest>().NotCompleted();
            Debug.Log("Already Reward");
        }
    }

    public void WeekJoin_Reward()
    {

        DateTime nowDate = DateTime.Now;

        TimeSpan difference = nowDate - savedDateTime_Week;
        if (difference.TotalDays > 7)
        {
            Debug.Log("Reward");

            savedDateTime_Daily = nowDate;
            weekJoinQueset.Completed();
            SavePlayerData_week();
        }
        else
        {
            weekJoinQueset.NotCompleted();
            Debug.Log("Already Reward");
        }
    }



    public void QuestOn()
    {
        gameObject.SetActive(true);
    }

    public void QuestOff()
    {
        gameObject.SetActive(false);
    }


    int ConvertBool_Toint(bool par) {
        if (par == true)
            return 1;
        else
            return 0;

     }

    bool Convertint_ToBool(int par)
    {
        if (par == 1)
            return true;
        else
            return false;

    }


}
