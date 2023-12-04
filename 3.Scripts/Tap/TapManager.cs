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
    public Transform[] tapBtnPosition = new Transform[tapCount]; // ��ư��
    public Transform[] tapContentPosition = new Transform[tapCount]; // ��������
    [SerializeField] Transform TapBoxStartPosition, TapBoxEndPosition; 
    // 0�� attack, 1�� defense

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


        if(isScreenTop) // ���� ��ũ�� ��ġ�� top �� ��� -> bottom ���� �ٲ�
        {
            TapManagerObject.position = TapBoxStartPosition.position;
        }
        else // ���� ��ũ�� ��ġ�� bottom �� ��� -> top ���� �ٲ�
        {
            TapManagerObject.position = TapBoxEndPosition.position;
        }


        isScreenTop = !isScreenTop;
    }

}