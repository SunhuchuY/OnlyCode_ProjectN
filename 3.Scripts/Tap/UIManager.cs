using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Kino;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text bloodStoneText;
    [SerializeField] TMP_Text soulFragmentText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text expText;
    [SerializeField] Image expbarSprite;

    float expDeclineDuration = 0.5f;

    BigInteger currentExp = 0,maxExp = 30;
    BigInteger[] maxExpList = {
    30, 68, 114, 184, 270, 744, 1120, 1680, 2484, 2484,
    3726, 3726, 3726, 3726, 4470, 5364, 6435, 7722, 9264,
    11115, 13338, 16005, 19206, 23046, 27654, 33183, 39819,
    47781, 57336, 57336, 76448, 76448, 76448, 76448, 91736,
    110080, 132096, 158512, 190212, 205428, 221860, 239608,
    258776, 279476, 301832, 325976, 352052, 380216, 410632,
    443480, 478956, 517272, 558652, 603344, 651608, 703736,
    760032, 820832, 886496, 1108120, 1108120, 1108120, 1108120,
    1108120, 1191225, 1280565, 1376605, 1479850, 1590835, 1710145,
    1838405, 1976285, 2124505, 2283840, 2443705, 2614760, 2797790,
    2993635, 3203185, 3427405, 3667320, 3924030, 4198710, 4492615,
    4807095, 5143590, 5503640, 5888890, 6301110, 6710680, 7146870,
    7611415, 8106155, 8633055, 9194200, 9791820, 10428285, 11106120,
    11828015, 11828015, 11828015, 11828015, 11828015, 11828015, 12596835,
    13415625, 14287640, 15216335, 16205395, 17258745, 18380560, 19575295,
    20847685, 22202780, 23645960, 25182945, 26819835, 28563120, 30419720,
    32397000, 34502805, 36745485, 39133940, 41677645, 44386690, 47271820,
    50344485, 53616875, 57101970, 60813595, 64766475, 68976295, 73459750,
    78234630, 83319880, 88735670, 94503485, 100646210, 107188210, 113887470,
    121005435, 128568270, 136603785, 145141520, 154212865, 163851165, 174091860,
    184972600, 196533385, 208816720, 221867765, 235734500, 250467905, 266122145,
    282754775, 300426945, 319203625, 339153850, 360350965, 382872900, 406802455,
    432227605, 459241830, 487944440, 518440965, 550843525, 585271245, 621850695,
    660716360, 693752175, 728439780, 764861765, 803104850, 843260090, 885423090,
    929694240, 976178950, 1024987895, 1076237285, 1130049145, 1186551600, 1245879180,
    1308173135, 1373581790, 1442260875, 1514373915, 1590092610, 1669597240, 1753077100,
    1840730955, 1932767500, 2029405875, 2130876165, 2237419970, 2349290965, 2466755510,
    2590093285, 2719597945, 2855577840
    };

    [DoNotSerialize] public int level = 1;

    public BigInteger soulFragment { get; internal set; } = 0;
    public BigInteger gold { get; internal set; } = 0;
    public BigInteger bloodStone { get; internal set; } = 0;

    private int goldRand_Start = 0, goldRand_End = 5;

    private bool istempsoulFragment = false, istempgold  = false, istempbloodStone = false;




    public BigInteger GetGold() {  return gold; }
    public BigInteger GetSoulFragment() { return soulFragment; }

    public BigInteger GetBloodStone() { return bloodStone; }

    public void SetGold(BigInteger input) { StartCoroutine(SlowChange_Item_Coroutine(gold, gold + input, RewardEnum.gold)); }
    public void SetSoulFragment(BigInteger input) { StartCoroutine(SlowChange_Item_Coroutine(soulFragment, soulFragment + input, RewardEnum.soulfragment)); }
    public void SetBloodStone(BigInteger input) { StartCoroutine(SlowChange_Item_Coroutine(bloodStone, bloodStone + input, RewardEnum.bloodstone)); }
        
    IEnumerator SlowChange_Item_Coroutine(BigInteger currentValue, BigInteger targetValue, RewardEnum reward)
    {
        float delay = 0.00001f;

        BigInteger temp = (BigInteger)((double)(targetValue - currentValue) * 0.9);
        switch (reward)
        {
            case RewardEnum.gold:
                gold += temp;
                break;
            case RewardEnum.bloodstone:
                bloodStone += temp;

                break;
            case RewardEnum.soulfragment:
                soulFragment += temp;

                break;
        }

        temp = (BigInteger)((double)(targetValue - currentValue) * 0.1);
        for (int i = 0; i < temp; i++)
        {
            switch (reward)
            {
                case RewardEnum.gold:
                    gold++;
                    break;
                case RewardEnum.bloodstone:
                    bloodStone++;
                    break;
                case RewardEnum.soulfragment:
                    soulFragment++;
                    break;
            }
            yield return new WaitForSeconds(delay);
        }
        
    }

    private void Update()
    {

        TextSet();

        if (level < maxExpList.Length)
        {
            if (currentExp >= maxExp)
            {
                var lastExp = currentExp - maxExp;

                level++;
                currentExp = lastExp;
                maxExp = maxExpList[level - 1];
            }


            var scaleX = (float)currentExp / (float)maxExp;
            expbarSprite.transform.DOScaleX(scaleX, expDeclineDuration);
        }

    }

    // parameter is Amount
    public void DropReward(BigInteger _EXP, BigInteger _bloodStone, BigInteger _gold, monsterType _monsterType)
    {

        // exp, bloodstone 은 기본
        currentExp += _EXP;
        SetBloodStone(_bloodStone);

        GameManager.Instance.accuireBoxManager.Appear_AccuireBox(BoxEnum.exp, _EXP);
        GameManager.Instance.accuireBoxManager.Appear_AccuireBox(BoxEnum.bloodStone, _bloodStone);

        switch (_monsterType)
        {
            case monsterType.Basic:
                if (GameManager.RandomRange(goldRand_Start , goldRand_End))
                {
                    // gold 는 특별드랍
                    SetGold(_gold);
                    GameManager.Instance.accuireBoxManager.Appear_AccuireBox(BoxEnum.gold, _gold);
                }
                break;
            case monsterType.Unique:
                break;
        }
    }

    void TextSet()
    {
        bloodStoneText.text = goldText.text = ToCurrencyString(bloodStone);
        soulFragmentText.text = ToCurrencyString(soulFragment);
        levelText.text = $"Lv.{level}";  
        goldText.text = ToCurrencyString(gold);
        expText.text = $"{ToCurrencyString(currentExp)}/{ToCurrencyString(maxExp)}";
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
}