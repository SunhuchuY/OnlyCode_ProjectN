using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // ��鸲 ������ ���� �ð��� ������ ����
    public float shakeStrength = 1f;
    public float shakeDuration = 1f;

    private void Start()
    {
        ShakeScreen();
    }
        

    GenericClass<int> intContainer = new GenericClass<int>(42);

    void ShakeScreen()
    {
        // Camera.main�� ���� Ȱ��ȭ�� ���� ī�޶� ����Ŵ
        Camera.main.transform.DOShakePosition(shakeDuration, shakeStrength);
    }

}

public class GenericClass<T> where T : struct
{
    private T data;

    public GenericClass(T data)
    {
        this.data = data;
    }

    public T GetData()
    {
        return data;
    }
}