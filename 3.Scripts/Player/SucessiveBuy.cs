using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SucessiveBuy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool isButtonPressed = false;

    // ��ư�� ���� �� ȣ��Ǵ� �Լ�
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    // ��ư�� �� �� ȣ��Ǵ� �Լ�
    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }
}
