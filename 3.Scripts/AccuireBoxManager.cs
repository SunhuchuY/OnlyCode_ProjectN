using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class AccuireBoxManager : MonoBehaviour
{

    const int Mount = 100;
    const int boxMount = 3;
    int count = 0;

    GameObject[] boxObjs = new GameObject[boxMount];
    public Sprite[] appearSprite = new Sprite[4];
    // 0 : 골드
    // 1 : 마석
    // 2 : 영혼조각
    // 3 : 경험치 

    bool[] isEmpty = new bool[boxMount];

    Color originalColor;

    private void Start()
    {
        for (int i = 0; i < isEmpty.Length; i++)
        {
            boxObjs[i] = transform.GetChild(i).gameObject;
        }
        originalColor = boxObjs[0].GetComponent<Image>().color;


    }


    string isAct = "isAct";
    public void Appear_AccuireBox(BoxEnum boxEnum, int value)
    {
        if (count >= 3)
            count = 0;

        AccuireBox accuireBox = boxObjs[count].GetComponent<AccuireBox>();

        if (boxObjs[count].activeSelf == false)
            boxObjs[count].SetActive(true);


        boxObjs[count].GetComponent<Image>().color = originalColor;
        boxObjs[count].GetComponent<Image>().DOFade(0.2f, 1f);



        switch (boxEnum)
        {
            case BoxEnum.gold:
                accuireBox.image.sprite = appearSprite[0];
                accuireBox.showText.text = $"골드";
                break;
            case BoxEnum.bloodStone:
                accuireBox.image.sprite = appearSprite[1];
                accuireBox.showText.text = $"마석";
                break;
            case BoxEnum.soulFragment:
                accuireBox.image.sprite = appearSprite[2];
                accuireBox.showText.text = $"영혼조각";
                break;
            case BoxEnum.exp:
                accuireBox.image.sprite = appearSprite[3];
                accuireBox.showText.text = $"경험치";
                break;
        }

        // 공통 적용
        accuireBox.numText.text = $"{ToCurrencyString(value)}";
        count++;

    }

    public void Appear_AccuireBox(BoxEnum boxEnum, BigInteger value)
    {
        if (count >= 3)
            count = 0;

        AccuireBox accuireBox = boxObjs[count].GetComponent<AccuireBox>();

        if (boxObjs[count].activeSelf == false)
            boxObjs[count].SetActive(true);


        boxObjs[count].GetComponent<Image>().color = originalColor;
        boxObjs[count].GetComponent<Image>().DOFade(0.2f, 1f);


        switch (boxEnum)
        {
            case BoxEnum.gold:
                accuireBox.image.sprite = appearSprite[0];
                accuireBox.showText.text = $"골드";
                break;
            case BoxEnum.bloodStone:
                accuireBox.image.sprite = appearSprite[1];
                accuireBox.showText.text = $"혈석";
                break;
            case BoxEnum.soulFragment:
                accuireBox.image.sprite = appearSprite[2];
                accuireBox.showText.text = $"영혼조각";
                break;
            case BoxEnum.exp:
                accuireBox.image.sprite = appearSprite[3];
                accuireBox.showText.text = $"경험치";
                break;
        }

        accuireBox.numText.text = $"{ToCurrencyString(value)}";
        count++;
    }

    /*private void Start()
    {
        for (int i = 0; i < Mount; i++)
        {
            GameObject obj = Instantiate(boxPrefeb);
            obj.transform.parent = transform;
            obj.SetActive(false);
        }
    }

    public void Appear_AccuireBox(BoxEnum boxEnum, int value)
    {

        if (emptySearchObj() == null)
            return;

        GameObject obj =  emptySearchObj();
        AccuireBox accuireBox = obj.GetComponent<AccuireBox>(); 


        switch (boxEnum)
        {
            case BoxEnum.gold:
                accuireBox.showText.text = $"골드";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.bloodStone:
                accuireBox.showText.text = $"혈석";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.soulFragment:
                accuireBox.showText.text = $"영혼조각";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.exp:
                accuireBox.showText.text = $"경험치";
                accuireBox.numText.text = $"{value}";
                break;
        }

    }

    GameObject emptySearchObj()
    {
        for (int i = 0; i < Mount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            if (obj.activeSelf == false)
            {
                obj.transform.position = transform.position;
                obj.SetActive(true);
                return obj;
            }

        }

        return null;
    }*/

    public float duration = 1f;
    void InitializeTween()
    {
        // 화면 너비를 기준으로 왼쪽에서 오른쪽으로 이동하는 Tweener 생성
        Tween flashTween = transform.DOMoveX(Screen.width, duration)
            .SetEase(Ease.InOutQuint)  // easeInOutQuint 이징 사용
            .SetAutoKill(false)        // 자동으로 Kill 되지 않도록 설정
            .Pause();                  // 일시 정지 상태로 시작

        // 반복되면서 Tweener를 실행하는 함수를 정의
        void FlashLoop()
        {
            // Tweener를 리셋하고 다시 시작
            flashTween.Rewind();
            flashTween.Restart();
        }

        // Tweener가 완료될 때마다 FlashLoop 함수를 호출하도록 설정
        flashTween.OnComplete(FlashLoop);

        // 초기에 한 번 실행하고 일시 정지 상태로 시작
        FlashLoop();
    }

    // 외부에서 이 함수를 호출하여 애니메이션을 시작
    public void StartFlash()
    {
        // Tweener를 재생
        transform.DOMoveX(Screen.width, duration)
            .SetEase(Ease.InOutQuint)
            .OnComplete(() =>
            {
                // 애니메이션이 완료된 후에 할 작업들
            });
    }

    public static string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    /// double 형 데이터를 클리커 게임의 화폐 단위로 표현
    public static string ToCurrencyString(BigInteger number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }
    public static string ToCurrencyString(float number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    public static string ToCurrencyString(double number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    public static string ToCurrencyString(int number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }
}
