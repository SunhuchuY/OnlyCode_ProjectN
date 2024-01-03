using System.Numerics;
using Random = UnityEngine.Random;

public class UserDataManager
{
    public static BigInteger[] MAX_EXP_LIST =
    {
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

    private string username = "Player";
    private int avatarId = 1;
    private int currentLevel = 1;
    private BigInteger gold { get; set; }
    private BigInteger magicstone { get; set; }
    private BigInteger diamond { get; set; }
    private BigInteger currentExp = 0;

    public string Username => username;
    public int AvatarId => avatarId;
    public int CurrentLevel => currentLevel;

    public BigInteger Gold
    {
        get => gold;
        set => gold = value;
    }

    public BigInteger Magicstone
    {
        get => magicstone;
        set => magicstone = value;
    }

    public BigInteger Diamond
    {
        get => diamond;
        set => diamond = value;
    }

    public BigInteger CurrentExp
    {
        get => currentExp;
        set => currentExp = value;
    }

    public BigInteger MaxExp => MAX_EXP_LIST[currentLevel - 1];

    public event System.Action OnUsernameChanged;
    public event System.Action OnAvatarIdChanged;
    public event System.Action OnCurrentLevelChanged;
    public event System.Action<BigInteger> OnGoldChanged;
    public event System.Action<BigInteger> OnMagicstoneChanged;
    public event System.Action<BigInteger> OnDiamondChanged;
    public event System.Action<BigInteger> OnCurrentExpChanged;
    public event System.Action<BigInteger> OnMaxExpChanged;

    public void GetReward(BigInteger _exp, BigInteger _magicstone, BigInteger _gold, monsterType _monsterType)
    {
        if (_exp > 0)
        {
            currentExp += _exp;
            OnCurrentExpChanged?.Invoke(_exp);

            if (currentExp >= MaxExp)
            {
                currentLevel++;
                OnCurrentLevelChanged?.Invoke();
            }
        }

        if (_magicstone > 0)
        {
            magicstone += _magicstone;
            OnMagicstoneChanged?.Invoke(_magicstone);
        }

        // temp: 임시적으로 골드 드랍률을 20%로 설정합니다.
        if (_gold > 0 && Random.Range(0f, 1f) > 0.2f)
        {
            if (_monsterType == monsterType.Basic)
            {
                gold += _gold;
                OnGoldChanged?.Invoke(_gold);
            }
        }
    }
}