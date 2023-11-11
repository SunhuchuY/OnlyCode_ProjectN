using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class HealthTap : MonoBehaviour
{

    int upForSecondsHp_Mount = 10, upMaxHealth_Mount = 100; // 각각 체력 회복량, 최대체력증가량
    int reqCurHealth_bloodstone = 300, reqMaxHealth_bloodstone = 500; // 각각 혈석 필요량 

    // 현재 능력치 텍스트
    [SerializeField]
    TMP_Text CurHealth_Text; // 현재 체력회복
    [SerializeField]
    TMP_Text MaxHealth_Text; // 현재 체력 MAX 증가

    // 필요골드 텍스트
    [SerializeField]
    TMP_Text reqCurHealth_Text;
    [SerializeField]
    TMP_Text reqMaxHealth_Text;

    Dictionary<HealthTapEnum, BigInteger[]> reqHealthTapDictionary = new Dictionary<HealthTapEnum, BigInteger[]>();

    private void Start()
    {
        reqHealthTapDictionary.Add(HealthTapEnum.upCurHealth, new BigInteger[]
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

        reqHealthTapDictionary.Add(HealthTapEnum.upMaxHealth, new BigInteger[]
        {
            100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200,
    102400, 204800, 409600, 819200, 1638400, 3276800, 6553600,
    13107200, 26214400, 52428800, 104857600, 209715200,
    419430400, 838860800, 1677721600, 3355443200, 6710886400,
    13421772800, 26843545600, 268435545600, 268435545600, 268435645600,
    268435745600, 268435845600, 268435945600, 268436045600, 268436145600,
    268436245600, 268436345600, 268436445600, 268436545600, 268436645600,
    268436745600, 268436845600, 268436945600, 268437045600, 268437145600,
    268437245600, 268437345600, 268437445600, 268437545600, 268437645600,
    268437745600, 268437845600, 268437945600, 268438045600, 268438145600,
    268438245600, 268438345600, 268438445600, 268438545600, 268438645600,
    268438745600, 268438845600, 268438945600, 268439045600, 268439145600,
    268439245600, 268439345600, 268439445600, 268439545600, 268439645600,
    268439745600, 268439845600, 268439945600, 268440045600, 268440145600,
    268440245600, 268440345600, 268440445600, 268440545600, 268440645600,
    268440745600, 268440845600, 268440945600, 268441045600, 268441145600,
    268441245600, 268441345600, 268441445600, 268441545600, 268441645600,
    268441745600, 268441845600, 268441945600, 268442045600, 268442145600,
    268442245600, 268442345600, 268442445600, 268442545600, 268442645600,
    268442755600, 268443845600, 268454545600, 268465545600, 268475545600,
    268485545600, 268495545600, 268505545600, 268515545600, 268525545600,
    268535545600, 268545545600, 268555545600, 268565545600, 268575545600,
    268585545600, 268595545600, 268605545600, 268615545600, 268625545600,
    268635545600, 268645545600, 268655545600, 268665545600, 268675545600,
    268685545600, 268695545600, 268705545600, 268715545600, 268725545600,
    268735545600, 268735545600, 268745545600, 268755545600, 268765545600,
    268775545600, 268785545600, 268795545600, 268805545600, 268815545600,
    268825545600, 268835545600, 268845545600, 268855545600, 268865545600,
    268875545600, 268885545600, 268895545600, 268905545600, 268915545600,
    268925545600, 268935545600, 268945545600, 268955545600, 268965545600,
    268975545600, 268985545600, 268995545600, 269005545600, 269015545600,
    269025545600, 269035545600, 269045545600, 269055545600, 269065545600,
    269075545600, 269085545600, 269095545600, 269105545600, 269115545600,
    269125545600, 269135545600, 269145545600, 269155545600, 269165545600,
    269175545600, 269185545600, 269195545600, 269205545600, 269215545600,
    269225545600, 269235545600, 269245545600, 269255545600, 269265545600,
    269275545600, 269285545600, 269295545600, 269305545600, 269315545600,
    269325545600, 269335545600, 269345545600, 269355545600, 269365545600,
    269375545600, 269385545600, 269395545600, 269405545600, 269415545600,
    269425545600, 269435545600, 269445545600, 269455545600, 269465545600,
    269475545600, 269485545600, 269495545600, 269505545600, 269515545600,
    269525545600, 269535545600, 269545545600, 269545545600, 270545545600,
    271545545600, 272545545600, 273545545600, 274545545600, 275545545600,
    276545545600, 27665545600, 27774545600, 28854545600, 29854545600,
    30854545600, 31854545600, 32854545600, 33854545600, 34854545600,
    35854545600, 36854545600, 37854545600, 38854545600, 39854545600,
    40854545600, 41854545600, 42854545600, 43854545600, 44854545600,
    45854545600, 46854545600, 47854545600, 48854545600, 49854545600,
    50854545600, 51854545600, 52854545600, 53854545600, 54854545600,
    55854545600, 66854545600, 81422754720, 98129596464, 117248696156,
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

        CurHealth_Text.text = $"10초에 한번씩 {ToCurrencyString(GameManager.playerScript.forsecondsUpAmount)}로 회복 증가";
        MaxHealth_Text.text = $"{ToCurrencyString(upMaxHealth_Mount)}만큼 최대 체력 증가";

        reqCurHealth_Text.text = $"{ToCurrencyString(reqHealthTapDictionary[HealthTapEnum.upCurHealth][0])}";
        reqMaxHealth_Text.text = $"{ToCurrencyString(reqHealthTapDictionary[HealthTapEnum.upMaxHealth][0])}";
    }

    public void upCurHealth_Btn()
    {
        UpAmount(HealthTapEnum.upCurHealth);
    }

    public void upMaxHealth_Btn() 
    {
        if (GameManager.playerScript.GetcurrentHealth() >= GameManager.playerScript.GetMaxHealth())
            return;

        UpAmount(HealthTapEnum.upMaxHealth);
    }



        void UpAmount(HealthTapEnum healthTapEnum)
        {
            inputStr = healthTapEnum;

            int level = 0;
            BigInteger nextrequiredBloodStone = 0;

            // level setting
            switch (healthTapEnum)
            {
                case HealthTapEnum.upCurHealth:
                    level = GameManager.playerScript.forsecondsUpAmountLevel;
                    break;

                case HealthTapEnum.upMaxHealth:
                    level = GameManager.playerScript.maxHealthLevel;
                    break;
            }

            if (level >= reqHealthTapDictionary[healthTapEnum].Length)
                return;

            var requiredBloodStone = reqHealthTapDictionary[healthTapEnum][level - 1];

            if (GameManager.uIManager.GetBloodStone() < requiredBloodStone)
                return; // 구매조건 불충족

            if (reqHealthTapDictionary[healthTapEnum].Length <= level)
                return; // 레밸 조건 불충족

            nextrequiredBloodStone = reqHealthTapDictionary[healthTapEnum][level];

            switch (healthTapEnum)
            {
                case HealthTapEnum.upCurHealth:

                    // text setting
                    reqCurHealth_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                    CurHealth_Text.text = ToCurrencyString( GameManager.playerScript.forsecondsUpAmount);

                    // system setting
                    GameManager.playerScript.forsecondsUpAmount += upForSecondsHp_Mount;
                    GameManager.playerScript.forsecondsUpAmountLevel++;

                    break;
                case HealthTapEnum.upMaxHealth:
                // text setting
               

                reqMaxHealth_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                MaxHealth_Text.text = ToCurrencyString(GameManager.playerScript.sumMaxhp());

                // system setting
                GameManager.playerScript.SetMaxHealth((double)upMaxHealth_Mount);   
                GameManager.playerScript.maxHealthLevel++;
                
                    break;
            }

            GameManager.uIManager.SetBloodStone(-1 * requiredBloodStone);
            StartCoroutine(GameManager.playerScript.FadeInOut());
            isPress = true;
        }
    


    const float isPressStartTime = 2f;
    const float delayTime = 0.1f;
    float curStartTIme = 0; 
    float curDealyTime = 0;
    bool isPress = false;
    HealthTapEnum inputStr;

    public void IsPressFalse()
    {
        isPress = false;
        inputStr = HealthTapEnum.NULL;
        curDealyTime = 0;
        curStartTIme = 0;
    }


    private void Update()
    {
        if (isPress && inputStr != HealthTapEnum.NULL)
        {
            curStartTIme += Time.deltaTime;

            if (curStartTIme > isPressStartTime && inputStr != HealthTapEnum.NULL)
            {
                curDealyTime += Time.deltaTime;

                if (curDealyTime > delayTime)
                {
                    switch (inputStr)
                    {
                        case HealthTapEnum.upMaxHealth:
                            upMaxHealth_Btn();
                            break;
                        case HealthTapEnum.upCurHealth:
                            upCurHealth_Btn();
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

            //  소수 첫째자리까지만 출력한다.
            showNumber = temp.ToString("F1").Replace(".0", "");
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

            //  소수 첫째자리까지만 출력한다.
            showNumber = temp.ToString("F1").Replace(".0", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }
}

