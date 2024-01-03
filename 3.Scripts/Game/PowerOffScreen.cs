using DG.Tweening;
using TMPro;
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
        curWaveText.text = $"현재 웨이브 : {GameManager.Instance.waveManager.WaveIndex}";

        bloodstoneText.text = $"{GameManager.Instance.userDataManager.Magicstone}";
        goldText.text = $"{GameManager.Instance.userDataManager.Gold}";
        // soulFragmentText.text = $"{GameManager.Instance.uIManager.GetSoulFragment()}";

            if (exitSlider.value == 1f) // PC
            {
                gameObject.SetActive(false);
                exitSlider.value = 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                exitSlider.value = 0;
            }


        if (Input.touchCount > 0) // 모바일 클릭시작시
        {
            // 첫 번째 터치를 가져옴
            Touch touch = Input.GetTouch(0);

            // 터치가 시작될 때
            if (touch.phase == TouchPhase.Began && exitSlider.value == 1f)
            {
                gameObject.SetActive(false);
                exitSlider.value = 0;
            }

            if (touch.phase == TouchPhase.Ended) // 모바일 클릭을떼어낼시
            {
                exitSlider.value = 0;
            }
        }

    }

    void SetTransparencyAnimation()
    {
        // 투명도가 30%에서 100%로 반복하도록 설정
        float toAlpha = 1.0f;
        float duration = 1.0f;

        // DoTween을 사용하여 애니메이션 설정
        sliderParent_Image.DOFade(toAlpha, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    public void On_PowerOffMode()
    {
        gameObject.SetActive(true);
    }

}
