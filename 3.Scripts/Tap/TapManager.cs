using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapManager : MonoBehaviour
{

    const int tapCount = 4;

    public Transform TapManagerObject;
    public Transform[] tapBtnPosition = new Transform[tapCount]; // 버튼별
    public Transform[] tapContentPosition = new Transform[tapCount]; // 콘텐츠별
    [SerializeField] Transform TapBoxStartPosition, TapBoxEndPosition; 
    // 0이 attack, 1이 defense

    bool isScreenTop = false;

    public void PickBtn(int tapNum)
    {

        for (int i = 0; i < tapCount; i++)
        {
            if (tapContentPosition[i] == null)
                continue;

            tapContentPosition[i].gameObject.SetActive(false);
        }

        tapContentPosition[tapNum].gameObject.SetActive(true);
    }

    public void TapManagerScreen_Move(BaseEventData data) // up and down drag
    {


        if(isScreenTop) // 현재 스크린 위치가 top 인 경우 -> bottom 으로 바꿈
        {
            TapManagerObject.position = TapBoxStartPosition.position;
        }
        else // 현재 스크린 위치가 bottom 인 경우 -> top 으로 바꿈
        {
            TapManagerObject.position = TapBoxEndPosition.position;
        }


        isScreenTop = !isScreenTop;
    }

}