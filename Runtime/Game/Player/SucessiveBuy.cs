using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SucessiveBuy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool isButtonPressed = false;

    // 버튼을 누를 때 호출되는 함수
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    // 버튼을 뗄 때 호출되는 함수
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }
}
