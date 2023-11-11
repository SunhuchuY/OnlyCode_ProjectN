using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.Universal;
using System;

public class GambleSystem : MonoBehaviour
{
    [SerializeField] GameObject gamblePanel_World, gamblePanel_Screen, touchPanel_Button, openALLCard, cardPrefab, openALLCard_Contents;

    [SerializeField] Transform showSprite_Transform;

    [SerializeField] Color ssrColor, srColor, rColor, beforeStartColor;

    [SerializeField] Light2D glowLight2D;

    [SerializeField] TMP_Text showText;

    [SerializeField] Sprite cardBackSprite;

    float rotationDuration = 4f, shakeDuration = 2f, shakeStrength = 1f;

    const int cardNumOfMax = 30;
    int rateOfR = 30, rateOfSR = 30, rateOfSSR = 10;
    int curnumofCard = -1;

    Dictionary<cardRateEnum, List<int>> cardRateDict = new Dictionary<cardRateEnum, List<int>>();

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(showText.DOFade(0.4f, 1.0f)); // 알파 값을 40%로 설정
        sequence.Append(showText.DOFade(1.0f, 1.0f)); // 알파 값을 100%로 설정
        sequence.SetLoops(-1); // 무한 반복

        for (int i = 0; i < cardNumOfMax; i++)
        {
            Instantiate(cardPrefab, openALLCard_Contents.transform);
        }

        cardRateDict.Add(cardRateEnum.R, new List<int> { 1, 2, 3, 4, 5, 6 });
        cardRateDict.Add(cardRateEnum.SR, new List<int> { 7, 8, 9 });
        cardRateDict.Add(cardRateEnum.SSR, new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 });
    }

    public void GambleOn()
    {
        gamblePanel_World.SetActive(true);
        gamblePanel_Screen.SetActive(true);
        touchPanel_Button.SetActive(true);
        showSprite_Transform.gameObject.SetActive(true);
        showText.gameObject.SetActive(true);

        glowLight2D.enabled = false; 
        glowLight2D.color = beforeStartColor;
        glowLight2D.intensity = 0.2f;
        showSprite_Transform.GetComponent<SpriteRenderer>().material.SetFloat("dissolveAmount", 1);
    }

    public void SettingNumberOfCard(int numofCard) // button
    {
        curnumofCard = numofCard;
        GambleOn();
    }

    public void GamebleSystem_Start()
    {
        if (curnumofCard <= 0 )
            return;

        if (curnumofCard > cardNumOfMax)
            return;

        int rand, maxRate = -1;

        int intR = rateOfR, intSR = rateOfSR+rateOfR, intSSR = rateOfSR + rateOfR+rateOfSSR;

        touchPanel_Button.SetActive(false);
        showText.gameObject.SetActive(false);
        glowLight2D.enabled = true;

        for (int i = 0; i < cardNumOfMax; i++)
        {
            openALLCard_Contents.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < curnumofCard; i++)
        {
            rand = UnityEngine.Random.Range(0, 101);
            GambleCard cardScript = openALLCard_Contents.transform.GetChild(i).GetComponent<GambleCard>();

            if (rand >= 0 && intR > rand)
            {
                if(maxRate < (int)cardRateEnum.R)
                    maxRate = 0;

                cardScript.cardIndex = GetRandomCardIndex(cardRateEnum.R);
            }
            else if (rand >= intR && intSR > rand)
            {
                if (maxRate < (int)cardRateEnum.SR)
                    maxRate = (int)cardRateEnum.SR;

                cardScript.cardIndex = GetRandomCardIndex(cardRateEnum.SR);
            }
            else if (rand >= intSR && intSSR > rand)
            {

                if (maxRate < (int)cardRateEnum.SSR)
                    maxRate = (int)cardRateEnum.SSR;

                cardScript.cardIndex = GetRandomCardIndex(cardRateEnum.SSR);
            }

            GameManager.skillTreeManager.CurCardStates[cardScript.cardIndex].numOfCard++;
        }

        StartCoroutine(ShowStart_Coroutine(maxRate));
    }

    int GetRandomCardIndex(cardRateEnum rate)
    {
        List<int> cardIndices;

        if (cardRateDict.TryGetValue(rate, out cardIndices))
        {
            return cardIndices[UnityEngine.Random.Range(0, cardIndices.Count)];
        }

        return -1;
    }

    IEnumerator ShowStart_Coroutine(int maxRate)
    {
        float rotationSpeed = 180f;

        switch (maxRate)
        {
            case 0:
                glowLight2D.color = rColor;
                shakeStrength = 0;
                break;
            case 1:
                glowLight2D.color = srColor;
                shakeStrength = 0.6f;
                break;
            case 2:
                glowLight2D.color = ssrColor;
                shakeStrength = 1.2f;
                break;
        }


        glowLight2D.intensity = 1f;

        // DOTween을 사용하여 z 축만 무한 반복하는 애니메이션 설정
        showSprite_Transform.transform.rotation = new Quaternion(0, 0, 0, 0);
        Tween rotateTween = showSprite_Transform.DORotate(new Vector3(0f, 0f, 360f), 0.8f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                float deltaTime = Time.deltaTime;
                showSprite_Transform.Rotate(new Vector3(0f, 0f, rotationSpeed * deltaTime));
            });

        showSprite_Transform.DOShakePosition(shakeDuration, shakeStrength);

        yield return new WaitForSeconds(rotationDuration);


        float targetValue = 0f;
        float animationDuration = 2f;
        float floatValue = 1f;
        Material showSprite_Material = showSprite_Transform.GetComponent<SpriteRenderer>().material;
        glowLight2D.enabled = false;
        // DOTween을 사용하여 float 변수를 천천히 0부터 1까지 증가시키는 애니메이션 설정
        DOTween.To(() => floatValue, x => floatValue = x, targetValue, animationDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() => {
                // 애니메이션 중에 발생하는 갱신 작업
                showSprite_Material.SetFloat("dissolveAmount", floatValue);
            })
            .OnComplete(() => {
                rotateTween.Pause();
                OpenCardSet();
            });
    }

    void OpenCardSet()
    {
        openALLCard.SetActive(true);

        // init setting
        for (int i = 0; i < curnumofCard; i++)
        {
            openALLCard_Contents.transform.GetChild(i).gameObject.SetActive(true);

            GambleCard cardScript = openALLCard_Contents.transform.GetChild(i).GetComponent<GambleCard>();
            cardScript.cardImage.sprite = cardBackSprite;
        }
    }

    public void OpenCardExitButton()
    {
        gamblePanel_World.SetActive(false); 
        gamblePanel_Screen.SetActive(false);
        touchPanel_Button.SetActive(false);
        showSprite_Transform.gameObject.SetActive(false);
        showText.gameObject.SetActive(false);
        openALLCard.SetActive(false);
    }

    public void ALLCardCheckButton()
    {
        for (int i = 0; i < curnumofCard; i++)
        {
            GambleCard cardScript = openALLCard_Contents.transform.GetChild(i).GetComponent<GambleCard>();
            cardScript.ShowCardImage();
        }   
    }
}