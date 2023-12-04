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
    // 0 : ���
    // 1 : ����
    // 2 : ��ȥ����
    // 3 : ����ġ 

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
                accuireBox.showText.text = $"���";
                break;
            case BoxEnum.bloodStone:
                accuireBox.image.sprite = appearSprite[1];
                accuireBox.showText.text = $"����";
                break;
            case BoxEnum.soulFragment:
                accuireBox.image.sprite = appearSprite[2];
                accuireBox.showText.text = $"��ȥ����";
                break;
            case BoxEnum.exp:
                accuireBox.image.sprite = appearSprite[3];
                accuireBox.showText.text = $"����ġ";
                break;
        }

        // ���� ����
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
                accuireBox.showText.text = $"���";
                break;
            case BoxEnum.bloodStone:
                accuireBox.image.sprite = appearSprite[1];
                accuireBox.showText.text = $"����";
                break;
            case BoxEnum.soulFragment:
                accuireBox.image.sprite = appearSprite[2];
                accuireBox.showText.text = $"��ȥ����";
                break;
            case BoxEnum.exp:
                accuireBox.image.sprite = appearSprite[3];
                accuireBox.showText.text = $"����ġ";
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
                accuireBox.showText.text = $"���";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.bloodStone:
                accuireBox.showText.text = $"����";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.soulFragment:
                accuireBox.showText.text = $"��ȥ����";
                accuireBox.numText.text = $"{value}";
                break;
            case BoxEnum.exp:
                accuireBox.showText.text = $"����ġ";
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
        // ȭ�� �ʺ� �������� ���ʿ��� ���������� �̵��ϴ� Tweener ����
        Tween flashTween = transform.DOMoveX(Screen.width, duration)
            .SetEase(Ease.InOutQuint)  // easeInOutQuint ��¡ ���
            .SetAutoKill(false)        // �ڵ����� Kill ���� �ʵ��� ����
            .Pause();                  // �Ͻ� ���� ���·� ����

        // �ݺ��Ǹ鼭 Tweener�� �����ϴ� �Լ��� ����
        void FlashLoop()
        {
            // Tweener�� �����ϰ� �ٽ� ����
            flashTween.Rewind();
            flashTween.Restart();
        }

        // Tweener�� �Ϸ�� ������ FlashLoop �Լ��� ȣ���ϵ��� ����
        flashTween.OnComplete(FlashLoop);

        // �ʱ⿡ �� �� �����ϰ� �Ͻ� ���� ���·� ����
        FlashLoop();
    }

    // �ܺο��� �� �Լ��� ȣ���Ͽ� �ִϸ��̼��� ����
    public void StartFlash()
    {
        // Tweener�� ���
        transform.DOMoveX(Screen.width, duration)
            .SetEase(Ease.InOutQuint)
            .OnComplete(() =>
            {
                // �ִϸ��̼��� �Ϸ�� �Ŀ� �� �۾���
            });
    }

    public static string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    /// double �� �����͸� Ŭ��Ŀ ������ ȭ�� ������ ǥ��
    public static string ToCurrencyString(BigInteger number)
    {
        string zero = "0";

        if (-1 < number && number < 1)
        {
            return zero;
        }

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  1A �̸��� �׳� ǥ��
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  �Ҽ� ��°�ڸ������� ����Ѵ�.
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

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  1A �̸��� �׳� ǥ��
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  �Ҽ� ��°�ڸ������� ����Ѵ�.
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

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  1A �̸��� �׳� ǥ��
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  �Ҽ� ��°�ڸ������� ����Ѵ�.
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

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  1A �̸��� �׳� ǥ��
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate((decimal)number).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  �Ҽ� ��°�ڸ������� ����Ѵ�.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }
}
