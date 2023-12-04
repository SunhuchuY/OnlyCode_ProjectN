using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AttackTap : MonoBehaviour
{


    private float upAttackAmount = 20f; // 공격력 증가량

    private Dictionary<AttackTapEnum, BigInteger[]> reqAttackTapDictionary = new Dictionary<AttackTapEnum, BigInteger[]>();

    private float restrictedCoolTime = 0.3f; // 이 이하로 쿨타임 다운 불가능

    // 현재 능력치 텍스트
    [SerializeField]
    TMP_Text upAttack_Text;
    [SerializeField]
    TMP_Text upAttackSpeed_Text;
    [SerializeField]
    TMP_Text upAttackRange_Text;
    [SerializeField]
    TMP_Text upcriticalPercentText;
    [SerializeField]
    TMP_Text upcriticalAddDamage_Text;


    // 필요골드 텍스트
    [SerializeField]
    TMP_Text reqAttackSpeed_Text;
    [SerializeField]
    TMP_Text reqAttack_Text;
    [SerializeField]
    TMP_Text reqAttackRange_Text;
    [SerializeField]
    TMP_Text reqcriticalPercent_Text;
    [SerializeField]
    TMP_Text reqcriticalAddString_Text;

    private void Start()        
    {
        // required
        reqAttackTapDictionary.Add(AttackTapEnum.attack,new BigInteger[] 
        {
            100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200,
            102400, 204800, 409600, 819200, 1638400, 3276800, 6553600,
            13107200, 26214400, 52428800, 104857600, 209715200, 419430400,
            838860800, 1677721600, 3355443200, 6710886400, 13421772800,
            26843545600, 26843545600, 26853545600, 26863545600, 26873545600,
            26883545600, 26893545600, 26903545600, 26913545600, 26923545600,
            26933545600, 26943545600, 26953545600, 26963545600, 26973545600,
            26983545600, 26993545600, 27003545600, 27013545600, 27023545600,
            27033545600, 27043545600, 27053545600, 27063545600, 27073545600,
            27083545600, 27093545600, 27103545600, 27113545600, 27123545600,
            27133545600, 27143545600, 27153545600, 27163545600, 27173545600,
            27183545600, 27193545600, 27203545600, 27213545600, 27223545600,
            27233545600, 27243545600, 27253545600, 27263545600, 27273545600,
            27283545600, 27293545600, 27303545600, 27313545600, 27323545600,
            27333545600, 27343545600, 27353545600, 27363545600, 27373545600,
            27383545600, 27393545600, 27403545600, 27413545600, 27423545600,
            27433545600, 27443545600, 27453545600, 27463545600, 27473545600,
            27483545600, 27493545600, 27503545600, 27513545600, 27523545600,
            27533545600, 27543545600, 27543545600, 28543545600, 29543545600,
            30543545600, 31543545600, 32543545600, 33543545600, 34543545600,
            35543545600, 36543545600, 37543545600, 38543545600, 39543545600,
            40543545600, 41543545600, 42543545600, 43543545600, 44543545600,
            45543545600, 46543545600, 47543545600, 48543545600, 49543545600,
            50543545600, 51543545600, 52543545600, 53543545600, 54543545600,
            55543545600, 56543545600, 57543545600, 66578254720, 81422705664,
            97707246797, 117248696156, 140698435387, 168838122465, 202605746958,
            243126896349, 291752275619, 350102730743, 420123276892, 504147932270,
            604977518724, 725973022469, 871167626963, 1045401152355, 1254481382826,
            1505377659392, 1806453191270, 2167743829524, 2601292595429, 3121551114515,
            3745861337418, 4495033604901, 5394040325881, 6606672589549, 7928007107459,
            9513608528950, 11416330234741, 13699596281689, 16439515538027

        });

        reqAttackTapDictionary.Add(AttackTapEnum.attackSpeed, new BigInteger[]
        {
           10000, 100000, 500000, 1000000, 5000000, 10240000, 50240000,
            10485760000, 50485760000, 10737418240000, 21474836480000,
            42949672960000, 85899345920000, 8589934592000, 85899345920,
            1030792151040
        });

        reqAttackTapDictionary.Add(AttackTapEnum.attackRange, new BigInteger[]
        {
           2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 20000,
            40000, 80000, 160000, 320000, 640000, 1280000, 2560000, 5120000,
            10240000, 20480000, 40960000, 81920000, 163840000, 327680000,
            655360000, 1310720000, 2621440000, 5242880000, 10485760000,
            20971520000, 41943040000, 83886080000, 167772160000,
            335544320000, 671088640000, 1342177280000, 2684354560000,
            5368709120000, 10737418240000, 21474836480000, 42949672960000,
            85899345920000, 171798691840000, 343597383680000, 687194767360000,
            1374389534720000, 2748779069440000, 5497558138880000,
            10597558138880000
        });

        reqAttackTapDictionary.Add(AttackTapEnum.criticalPercent, new BigInteger[]
        {
             17083, 25624, 38436, 57654, 86481, 129721, 194581, 291871,
            437806, 656709, 985063, 1477594, 2216391, 3324586, 4986879,
            7480318, 11220477, 16830715, 25246072, 37869108, 56803662,
            85205493, 127808239, 191712358, 287568537, 431352805,
            647029207, 970543810, 1455815715, 2183723573, 3275585360,
            4913378040, 7370067060, 11055100590, 16582650885, 24873976328,
            37310964492, 55966446738, 83949670107, 167899340214,
            335798680428, 671597360856, 1343194721712, 2686389443424,
            5372778886848, 6447334664217, 7736801597061, 9284161916473,
            11140994299768, 13369193159721, 16043031791665,
            19251638149999, 23101965779998, 27722358935998,
            33266830723198, 39920196867838, 47904236241405, 57485083489686,
            68982100187624, 82778520225149, 99334224270179,
            119201069124214, 143041282949057, 171649539538869,
            205979447446643, 247175336935972, 296610404323166,
            355932485187799, 427118982225359, 512542778670431,
            615051334404518, 738061601285421, 1107092401928130,
            1660638602892190, 2490957904338290, 3736436856507440,
            5604655284761170, 6406982927141750, 7688379512570100,
            9226055415084120, 11071266498010090, 13285519797721100,
            15942623757265400, 19131148508718400, 22957378210462100,
            27548853852554500, 30303739237810000, 33334113161591000,
            36667524477750100, 40334276925052500, 44367704618077600
        });

        reqAttackTapDictionary.Add(AttackTapEnum.CriticalAddDamagePercent, new BigInteger[]
        {
            500, 1000, 1500, 2250, 3375, 5062, 7593, 11389, 17083, 25624,
            38436, 57654, 86481, 129721, 194581, 291871, 437806, 656709,
            985063, 1477594, 2216391, 3324586, 4986879, 7480318, 11220477,
            16830715, 25246072, 37869108, 56803662, 85205493, 127808239,
            191712358, 287568537, 431352805, 647029207, 970543810,
            1455815715, 2183723573, 3275585360, 4913378040, 7370067060,
            11055100590, 16582650885, 24873976328, 37310964492,
            55966446738, 83949670107, 167899340214, 335798680428,
            671597360856, 1343194721712, 2686389443424, 5372778886848,
            6447334664217, 7736801597061, 9284161916473, 11140994299768,
            13369193159721, 16043031791665, 19251638149999,
            23101965779998, 27722358935998, 33266830723198,
            39920196867838, 47904236241405, 57485083489686, 68982100187624,
            82778520225149, 99334224270179, 119201069124214,
            143041282949057, 171649539538869, 205979447446643,
            247175336935972, 296610404323166, 355932485187799,
            427118982225359, 512542778670431, 615051334404518,
            738061601285421, 1107092401928130, 1660638602892190,
            2490957904338290, 3736436856507440, 5604655284761170,
            6406982927141750, 7688379512570100, 9226055415084120,
            11071266498010090, 13285519797721100, 15942623757265400,
            19131148508718400, 22957378210462100, 27548853852554500,
            30303739237810000, 33334113161591000, 36667524477750100,
            40334276925052500, 44367704618077600
        });


        // amount
        reqAttack_Text.text = $"{ToCurrencyString(reqAttackTapDictionary[AttackTapEnum.attack][0])}";
        reqAttackSpeed_Text.text = $"{ToCurrencyString(reqAttackTapDictionary[AttackTapEnum.attackSpeed][0])}";
        reqAttackRange_Text.text = $"{ToCurrencyString(reqAttackTapDictionary[AttackTapEnum.attackRange][0])}";
        reqcriticalPercent_Text.text = $"{ToCurrencyString(reqAttackTapDictionary[AttackTapEnum.criticalPercent][0])}";
        reqcriticalAddString_Text.text = $"{ToCurrencyString(reqAttackTapDictionary[AttackTapEnum.CriticalAddDamagePercent][0])}";

        upAttack_Text.text = $"{ToCurrencyString( GameManager.Instance.playerScript.sumAttack())}";
        upAttackSpeed_Text.text = $"{ToCurrencyString(Mathf.Floor(GameManager.Instance.playerScript.sumSpeed() * 100) / 100)}";
        upAttackRange_Text.text = $"{ToCurrencyString( GameManager.Instance.playerScript.sumRange())}";
        upcriticalPercentText.text = $"{ToCurrencyString(GameManager.Instance.playerScript.GetCriticalPercent())} %";
        upcriticalAddDamage_Text.text = $"{ToCurrencyString(GameManager.Instance.playerScript.GetCriticalAddDamage())} %";
    }

    public void UpAttack()
    {
        UpAmount(AttackTapEnum.attack);
    }
    public void UpAttackSpeed()
    {
        UpAmount(AttackTapEnum.attackSpeed);
    }

    public void UpAttackRange()
    {
        UpAmount(AttackTapEnum.attackRange);
    }

    public void UpCriticalPercent()
    {
        UpAmount(AttackTapEnum.criticalPercent);
        
    }

    public void UpCriticalAddDamagePercent()
    {
        UpAmount(AttackTapEnum.CriticalAddDamagePercent);
        
    }

    void UpAmount(AttackTapEnum attackTapEnum)
    {
        inputStr = attackTapEnum;

        int level = 0;
        BigInteger nextrequiredBloodStone = 0;

        // level setting
        switch (attackTapEnum)
        {
            case AttackTapEnum.attack:
                level = GameManager.Instance.playerScript.attackLevel;
                break;
            case AttackTapEnum.attackRange:
                level = GameManager.Instance.playerScript.attackRankgeLevel;
                break;
            case AttackTapEnum.attackSpeed: 
                level = GameManager.Instance.playerScript.attackSpeedLevel;
                break;
            case AttackTapEnum.criticalPercent:
                level = GameManager.Instance.playerScript.criticalPercentLevel;
                break;
            case AttackTapEnum.CriticalAddDamagePercent:
                level = GameManager.Instance.playerScript.criticalAddDamagePercentLevel;
                break;
        }

        var requiredBloodStone = reqAttackTapDictionary[attackTapEnum][level - 1];

        if (level >= reqAttackTapDictionary[attackTapEnum].Length)
            return;

        if (GameManager.Instance.uIManager.GetBloodStone() < requiredBloodStone)
            return; // 구매조건 불충족

        if (reqAttackTapDictionary[attackTapEnum].Length <= level)
            return; // 레밸 조건 불충족

        nextrequiredBloodStone = reqAttackTapDictionary[attackTapEnum][level];

        switch (attackTapEnum)
        {
            case AttackTapEnum.attack:

                // text setting
                reqAttack_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                upAttack_Text.text = $"{ToCurrencyString(GameManager.Instance.playerScript.sumAttack())}";

                // system setting
                GameManager.Instance.playerScript.attack += upAttackAmount;
                GameManager.Instance.playerScript.attackLevel++;

                break;
            case AttackTapEnum.attackRange:
                // text setting
                reqAttackRange_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                upAttackRange_Text.text = ToCurrencyString(GameManager.Instance.playerScript.sumRange());

                // system setting
                GameManager.Instance.playerScript.attackRankgeLevel++;
                GameManager.Instance.playerScript.attackRankge++;
                float up = (GameManager.Instance.playerScript.sumRange() - 1) * GameManager.Instance.playerScript.upRange;

                GameManager.Instance.bulletController.gameObject.transform.localScale = new UnityEngine.Vector2(
                    GameManager.Instance.playerScript.initRange_X + up,
                    GameManager.Instance.playerScript.initRange_Y + up
                    );

                break;
            case AttackTapEnum.attackSpeed:
                // text setting
                reqAttackSpeed_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                upAttackSpeed_Text.text = $"{ToCurrencyString(Mathf.Floor(GameManager.Instance.playerScript.sumSpeed() * 100) / 100)}";

                // system setting
                GameManager.Instance.bulletController.SetCoolTime(-0.05f);
                GameManager.Instance.playerScript.attackSpeedLevel++;

                break;
            case AttackTapEnum.criticalPercent:
                // text setting
                reqcriticalPercent_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";
                upcriticalPercentText.text = $"{ToCurrencyString(Mathf.Floor(GameManager.Instance.playerScript.GetCriticalPercent() * 100) / 100)}";

                // system setting
                GameManager.Instance.playerScript.SetCriticalAddDamage(1);
                GameManager.Instance.playerScript.criticalPercentLevel++;

                break;
            case AttackTapEnum.CriticalAddDamagePercent:
                //text setting
                upcriticalAddDamage_Text.text = $"{ToCurrencyString(GameManager.Instance.playerScript.GetCriticalAddDamage())} %";
                reqcriticalAddString_Text.text = $"{ToCurrencyString(nextrequiredBloodStone)}";

                // system setting
                GameManager.Instance.playerScript.SetCriticalAddDamage(1);
                GameManager.Instance.playerScript.criticalAddDamagePercentLevel++;

                break;
        }

        GameManager.Instance.uIManager.SetBloodStone(-1 * requiredBloodStone);
        isPress = true;
    }

    readonly float isPressStartTime = 2f;
    readonly float delayTime = 0.1f;
    private float curStartTIme = 0;
    private float curDealyTime = 0;
    private bool isPress = false;
    private AttackTapEnum inputStr;


    public void IsPressFalse()
    {
        inputStr = AttackTapEnum.NULL;
        isPress = false;
        curDealyTime = 0;
        curStartTIme = 0;
    }

    private void Update()
    {
        if ( isPress && inputStr != AttackTapEnum.NULL)
        {
            curStartTIme += Time.deltaTime;

            if(curStartTIme > isPressStartTime )
            {
                curDealyTime += Time.deltaTime;

                if (curDealyTime > delayTime && inputStr != AttackTapEnum.NULL)
                {
                    switch (inputStr)
                    {   
                        case AttackTapEnum.attack:
                            UpAttack();
                            break;
                        case AttackTapEnum.attackRange:
                            UpAttackRange();
                            break;
                        case AttackTapEnum.attackSpeed:
                            UpAttackSpeed();
                            break;
                        case AttackTapEnum.criticalPercent:
                            UpCriticalPercent();
                            break;
                        case AttackTapEnum.CriticalAddDamagePercent:
                            UpCriticalAddDamagePercent();
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

            //  소수 둘째자리까지만 출력한다.
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
