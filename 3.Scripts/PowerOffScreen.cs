using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerOffScreen : MonoBehaviour
{
    [SerializeField] private Slider exitSlider;
    [SerializeField] private TMP_Text dayText, timeText ,curWaveText, bloodstoneText, goldText, soulFragmentText;

    private Image sliderParent_Image;

    private void Start()
    {
        sliderParent_Image= exitSlider.GetComponent<Image>();
    }

    private void Update()
    {
        timeText.text = System.DateTime.Now.ToString("HH:mm");
        dayText.text = System.DateTime.Now.ToString("yyyy-MM-dd");
        curWaveText.text = $"���� ���̺� : {GameManager.Instance.monsterControll.curWaveLev}";

        bloodstoneText.text = $"{GameManager.Instance.uIManager.GetBloodStone()}";
        goldText.text = $"{GameManager.Instance.uIManager.GetGold()}";
        soulFragmentText.text = $"{GameManager.Instance.uIManager.GetSoulFragment()}";

            if (exitSlider.value == 1f) // PC
            {
                gameObject.SetActive(false);
                exitSlider.value = 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                exitSlider.value = 0;
            }


        if (Input.touchCount > 0) // ����� Ŭ�����۽�
        {
            // ù ��° ��ġ�� ������
            Touch touch = Input.GetTouch(0);

            // ��ġ�� ���۵� ��
            if (touch.phase == TouchPhase.Began && exitSlider.value == 1f)
            {
                gameObject.SetActive(false);
                exitSlider.value = 0;
            }

            if (touch.phase == TouchPhase.Ended) // ����� Ŭ���������
            {
                exitSlider.value = 0;
            }
        }

    }

    void SetTransparencyAnimation()
    {
        // ������ 30%���� 100%�� �ݺ��ϵ��� ����
        float toAlpha = 1.0f;
        float duration = 1.0f;

        // DoTween�� ����Ͽ� �ִϸ��̼� ����
        sliderParent_Image.DOFade(toAlpha, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    public void On_PowerOffMode()
    {
        gameObject.SetActive(true);
    }

}
