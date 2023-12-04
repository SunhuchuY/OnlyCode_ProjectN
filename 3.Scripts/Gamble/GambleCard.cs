using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GambleCard : MonoBehaviour
{
    readonly private float ShowCardRotate_Duration = 0.25f;

    public Image cardImage;
    public int cardIndex;
    private bool isOpen;

    private void OnEnable()
    {
        isOpen = false;
    }

    public void ShowCardImage()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(ShowCardImage_Coroutine());
        }

    }

    IEnumerator ShowCardImage_Coroutine()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.DORotate(new Vector3(0, 90, 0), ShowCardRotate_Duration)  // 수정된 부분
            .SetEase(Ease.Linear); // 회전 애니메이션의 이징 설정 (선택사항)

        yield return new WaitForSeconds(ShowCardRotate_Duration);

        cardImage.sprite = GameManager.Instance.skillTreeManager.cardDatas[cardIndex].cardSprite;

        transform.DORotate(new Vector3(0, 0, 0), ShowCardRotate_Duration)  // 수정된 부분
            .SetEase(Ease.Linear); // 회전 애니메이션의 이징 설정 (선택사항)
    }
}
