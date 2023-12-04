using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // 흔들림 강도와 지속 시간을 조절할 변수
    public float shakeStrength = 1f;
    public float shakeDuration = 1f;

    private void Start()
    {
        ShakeScreen();
    }
        

    GenericClass<int> intContainer = new GenericClass<int>(42);

    void ShakeScreen()
    {
        // Camera.main은 현재 활성화된 메인 카메라를 가리킴
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