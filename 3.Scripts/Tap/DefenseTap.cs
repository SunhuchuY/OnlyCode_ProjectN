using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DefenseTap : MonoBehaviour
{
    int upCounterAmount = 5;
    int upDefenseAmount = 30;

    Dictionary<DefenseTapEnum, BigInteger[]> reqDefenseTapDictionary = new Dictionary<DefenseTapEnum, BigInteger[]>();

    // 현재 능력치 텍스트
    [SerializeField]
    TMP_Text upCounterAttack_Text;
    [SerializeField]
    TMP_Text upDefenseAmount_Text;

    // 필요골드 텍스트
    [SerializeField]
    TMP_Text reqCounterAttack_Text;
    [SerializeField]
    TMP_Text reqDefenseAmount_Text;

    private void Start()
    {
        reqDefenseTapDictionary.Add(DefenseTapEnum.defenseAmount, new BigInteger[]
        {
           100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200,
            102400, 204800, 409600, 819200, 1638400, 3276800, 6553600,
            13107200, 26214400, 52428800, 104857600, 209715200,
            419430400, 838860800, 1677721600, 3355443200, 6710886400,
            13421772800, 26843545600, 26843545600, 26853545600, 26863545600,
            26873545600, 26883545600, 26893545600, 26903545600, 26913545600,
            26923545600, 26933545600, 26943545600, 26953545600, 26963545600,
            26973545600, 26983545600, 26993545600, 27003545600, 27013545600,
            27023545600, 27033545600, 27043545600, 27053545600, 27063545600,
            27073545600, 27083545600, 27093545600, 27103545600, 27113545600,
            27123545600, 27133545600, 27143545600, 27153545600, 27163545600,
            27173545600, 27183545600, 27193545600, 27203545600, 27213545600,
            27223545600, 27233545600, 27243545600, 27253545600, 27263545600,
            27273545600, 27283545600, 27293545600, 27303545600, 27313545600,
            27323545600, 27333545600, 27343545600, 27353545600, 27354545600,
            27364545600, 27374545600, 27384545600, 27394545600, 27404545600,
            27414545600, 27424545600, 27434545600, 27444545600, 27454545600,
            27464545600, 27474545600, 27484545600, 27494545600, 27504545600,
            27514545600, 27524545600, 27534545600, 27544545600, 27554545600,
            27564545600, 27574545600, 27584545600, 27594545600, 27604545600,
            27614545600, 27624545600, 27634545600, 27644545600, 27654545600,
            27665545600, 27774545600, 28985545600, 29985545600, 30985545600,
            31985545600, 32985545600, 33985545600, 34985545600, 35985545600,
            36985545600, 37985545600, 38985545600, 39985545600, 40985545600,
            41985545600, 42985545600, 43985545600, 44985545600, 45985545600,
            46985545600, 47985545600, 48985545600, 49985545600, 50985545600,
            51985545600, 52985545600, 53985545600, 54985545600, 55985545600,
            66985545600, 81422754720, 98129596464, 117248696156,
            140698435387, 168838122465, 202605746958, 243126896349,
            291752275619, 350102730743, 420123276892, 504147932270,
            604977518724, 725973022469, 871167626963, 1045401152355,
            1254481382826, 1505377659392, 1806453191270, 2167743829524,
            2601292595429, 3121551114515, 3745861337418, 4495033604901,
            5394040325881, 6472848391058, 7767418069269, 9320901683123,
            11185082019748, 13422098423697, 16106518108437, 19327821730124,
            23193386076149, 27832063291379, 33398475949654, 40078171139585,
            48093805367502, 57712566441002, 69255079729203, 83106095675043,
            99727314810052, 119672777772062, 143607333326475,
            172328799991770, 206794559990124, 248153471988149,
            297784166385778, 357340999662934, 428809199595521,
            514571039514625, 617485247417550, 740982296901060,
            889178756281272, 1067014507537530, 1280417409045030,
            1536500890854040, 1843801069024850, 2212561282829810,
            2655073539395780, 3186088247274930, 3823305896729920,
            4587967076075900, 5505560491291080, 6606672589549300,
            7928007107459160, 9513608528950990, 11416330234741200,
            13699596281689400, 16439515538027300
        });

        reqDefenseTapDictionary.Add(DefenseTapEnum.counterAttack, new BigInteger[]
        {
            100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200,
            102400, 204800, 409600, 819200, 1638400, 3276800, 6553600,
            13107200, 26214400, 52428800, 104857600, 209715200,
            419430400, 838860800, 1677721600, 3355443200, 6710886400,
            13421772800, 26843545600, 26843545600, 26853545600, 26863545600,
            26873545600, 26883545600, 26893545600, 26903545600, 26913545600,
            26923545600, 26933545600, 26943545600, 26953545600, 26963545600,
            26973545600, 26983545600, 26993545600, 27003545600, 27013545600,
            27023545600, 27033545600, 27043545600, 27053545600, 27063545600,
            27073545600, 27083545600, 27093545600, 27103545600, 27113545600,
            27123545600, 27133545600, 27143545600, 27153545600, 27163545600,
            27173545600, 27183545600, 27193545600, 27203545600, 27213545600,
            27223545600, 27233545600, 27243545600, 27253545600, 27263545600,
            27273545600, 27283545600, 27293545600, 27303545600, 27313545600,
            27323545600, 27333545600, 27343545600, 27353545600, 27354545600,
            27364545600, 27374545600, 27384545600, 27394545600, 27404545600,
            27414545600, 27424545600, 27434545600, 27444545600, 27454545600,
            27464545600, 27474545600, 27484545600, 27494545600, 27504545600,
            27514545600, 27524545600, 27534545600, 27544545600, 27554545600,
            27564545600, 27574545600, 27584545600, 27594545600, 27604545600,
            27614545600, 27624545600, 27634545600, 27644545600, 27654545600,
            27665545600, 27774545600, 28854545600, 29854545600, 30854545600,
            31854545600, 32854545600, 33854545600, 34854545600, 35854545600,
            36854545600, 37854545600, 38854545600, 39854545600, 40854545600,
            41854545600, 42854545600, 43854545600, 44854545600, 45854545600,
            46854545600, 47854545600, 48854545600, 49854545600, 50854545600,
            51854545600, 52854545600, 53854545600, 54854545600, 55854545600,
            66854545600, 81422754720, 98129596464, 117248696156,
            140698435387, 168838122465, 202605746958, 243126896349,
            291752275619, 350102730743, 420123276892, 504147932270,
            604977518724, 725973022469, 871167626963, 1045401152355,
            1254481382826, 1505377659392, 1806453191270, 2167743829524,
            2601292595429, 3121551114515, 3745861337418, 4495033604901,
            5394040325881, 6472848391058, 7767418069269, 9320901683123,
            11185082019748, 13422098423697, 16106518108437, 19327821730124,
            23193386076149, 27832063291379, 33398475949654, 40078171139585,
            48093805367502, 57712566441002, 69255079729203, 83106095675043,
            99727314810052, 119672777772062, 143607333326475,
            172328799991770, 206794559990124, 248153471988149,
            297784166385778, 357340999662934, 428809199595521,
            514571039514625, 617485247417550, 740982296901060,
            889178756281272, 1067014507537530, 1280417409045030,
            1536500890854040, 1843801069024850, 2212561282829810,
            2655073539395780, 3186088247274930, 3823305896729920,
            4587967076075900, 5505560491291080, 6606672589549300,
            7928007107459160, 9513608528950990, 11416330234741200,
            13699596281689400, 16439515538027300
        });


        upCounterAttack_Text.text = $"{ToCurrencyString(GameManager.Instance.playerScript.sumCounter())}";
        upDefenseAmount_Text.text = $"{ToCurrencyString(GameManager.Instance.playerScript.GetDefenseAmount())}";

        reqCounterAttack_Text.text = $"{ToCurrencyString(reqDefenseTapDictionary[DefenseTapEnum.counterAttack][0])}";
        reqDefenseAmount_Text.text = $"{ToCurrencyString(reqDefenseTapDictionary[DefenseTapEnum.defenseAmount][0])}";

    }

    public void UpCounterAttack() //btn
    {
        UpAmount(DefenseTapEnum.counterAttack);
    }

    public void UpDefenseAmount() //btn
    {
        UpAmount(DefenseTapEnum.defenseAmount);
    }

    void UpAmount(DefenseTapEnum defenseTapEnum)
    {
        inputStr = defenseTapEnum;

        int level = 0;
        BigInteger nextrequiredBloodStone = 0;

        // level setting
        switch (defenseTapEnum)
        {
            case DefenseTapEnum.counterAttack:
                level = GameManager.Instance.playerScript.counterAttack_DamageLevel;
                break;

            case DefenseTapEnum.defenseAmount:
                level = GameManager.Instance.playerScript.defenseAmountLevel;
                break;
        }

        if (level >= reqDefenseTapDictionary[defenseTapEnum].Length)
            return;

        var requiredBloodStone = reqDefenseTapDictionary[defenseTapEnum][level - 1];

        if (GameManager.Instance.uIManager.GetBloodStone() < requiredBloodStone)
            return; // 구매조건 불충족

        if (reqDefenseTapDictionary[defenseTapEnum].Length <= level)
            return; // 레밸 조건 불충족

        nextrequiredBloodStone = reqDefenseTapDictionary[defenseTapEnum][level];

        switch (defenseTapEnum)
        {
            case DefenseTapEnum.counterAttack:

                // text setting
                reqCounterAttack_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}개가 필요합니다.";
                upCounterAttack_Text.text = ToCurrencyString(GameManager.Instance.playerScript.sumCounter());

                // system setting
                GameManager.Instance.playerScript.SetcounterAttack_Damage(upCounterAmount);
                GameManager.Instance.playerScript.counterAttack_DamageLevel++;


                break;
            case DefenseTapEnum.defenseAmount:
                // text setting
                reqDefenseAmount_Text.text = $"{ToCurrencyString( nextrequiredBloodStone)}개가 필요합니다.";
                upDefenseAmount_Text.text = ToCurrencyString(GameManager.Instance.playerScript.GetDefenseAmount());

                // system setting
                GameManager.Instance.playerScript.SetDefenseAmount(upDefenseAmount);
                GameManager.Instance.playerScript.defenseAmountLevel++;


                break;
        }

        GameManager.Instance.uIManager.SetBloodStone(-1 * requiredBloodStone);
        isPress = true;
    }

    const float isPressStartTime = 2f;
    const float delayTime = 0.125f;
    float curStartTIme = 0;
    float curDealyTime = 0;
    bool isPress = false;
    DefenseTapEnum inputStr;


    public void IsPressFalse()
    {
        isPress = false;
        inputStr = DefenseTapEnum.NULL;
        curDealyTime = 0;
        curStartTIme = 0;
    }

    private void Update()
    {
        if (isPress && inputStr != DefenseTapEnum.NULL)
        {
            curStartTIme += Time.deltaTime;

            if (curStartTIme > isPressStartTime && inputStr != DefenseTapEnum.NULL)
            {
                curDealyTime += Time.deltaTime;

                if (curDealyTime > delayTime )
                {
                    switch (inputStr)
                    {
                        case DefenseTapEnum.defenseAmount:
                            UpDefenseAmount();
                            break;
                        case DefenseTapEnum.counterAttack:
                            UpCounterAttack();
                            break;
                    }
                    curDealyTime = 0;
                }
            }
        }
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

            //  소수 첫째자리까지만 출력한다.
            showNumber = temp.ToString("F1").Replace(".0", "");
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

            //  소수 첫째자리까지만 출력한다.
            showNumber = temp.ToString("F1").Replace(".0", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

}
