using BackEnd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class SavedSkillData
{
    public int Count;
    public int Level;
}

public class SavedUserData
{
    //public string UserName;

    public string Diamond;
    public string Gold;
    public string MagicStone;

    public int CurrentLevel;
    public string CurrentExp;

    public int BigWave;
    public int Wave;

    public int MaxHealthLevel;
    public int RecoveryLevel;
    public int AttackLevel;
    public int AttackMultiplierLevel;
    public int AttackRangeLevel;
    public int AttackSpeedLevel;
    public int DefenseLevel;
    public int CriticalChanceLevel;
    public int CriticalMultiplierLevel;
    public int CounterAttackLevel;

    public string SkillsInUseList;
    public string SkillDict;
}

public class UserData : SavedUserData
{
    public new BigInteger Diamond;
    public new BigInteger Gold;
    public new BigInteger MagicStone;
    public new BigInteger CurrentExp;
    public new List<int> SkillsInUseList;
    public new Dictionary<int, SavedSkillData> SkillDict;
}

public class UserDataManager
{
    public SavedUserData savedUserData;
    public UserData userData = new();
    public bool backendConnected;

    const string USERDATA_TABLE_NAME = "userData";
    public BigInteger MaxExp => PlayerTapParser.Instance.levelOfPlayer[userData.CurrentLevel].Experience;

    public UserDataManager()
    {
        CreateUserData(userData);

        if (BackEnd.Backend.IsLogin)
        {

            if (FetchOrInsertFromBackend())
            {
                // 서버에서 사용자 정보 가져오기를 성공했을 때 실행됩니다.
                backendConnected = true;
            }
            else
            {
                Debug.LogError("백엔드에 로그인 되어있지만, 백엔드로부터 유저 데이터를 데이터를 가져오거나 추가하지 못했습니다.");
            }
        }
        else
        {
            // temp: 임시적으로 Backend에 로그인되지 않았을 때 로컬 json파일을 세이브로 불러오는 것으로 간주합니다.
            // 이 코드는 개발시 테스트할 때에만 실행되어야 하는 루틴이므로,
            // 사용자 앱에서 이쪽이 실행되지 않도록 Backend 로그인에 실패하면 게임 씬으로 진입하지 못하도록 막아야합니다.
#if UNITY_EDITOR
            if (FetchOrAddFromLocal())
            {
            }
#endif
        }
    }

    public void Update()
    {
        var _newSave = CreateSavedData(userData);

        if (backendConnected)
            UpdateToBackend(_newSave);
        else
#if UNITY_EDITOR
            UpdateToLocal(_newSave);
#endif

        savedUserData = _newSave;
    }

    private void CreateUserData(UserData _userData)
    {
        string _userName = Backend.IsLogin ? Backend.UserNickName : $"Player";

        //_userData.UserName = _userName;
        _userData.Diamond = 10;
        _userData.Gold = 10;
        _userData.MagicStone = 10;
        _userData.CurrentLevel = 1;
        _userData.CurrentExp = 0;
        _userData.BigWave = 1;
        _userData.Wave = 1;
        _userData.MaxHealthLevel = 1;
        _userData.RecoveryLevel = 1;
        _userData.AttackLevel = 1;
        _userData.AttackMultiplierLevel = 1;
        _userData.AttackRangeLevel = 1;
        _userData.AttackSpeedLevel = 1;
        _userData.DefenseLevel = 1;
        _userData.CriticalChanceLevel = 1;
        _userData.CriticalMultiplierLevel = 1;
        _userData.CounterAttackLevel = 1;
        _userData.SkillsInUseList = new();
        _userData.SkillDict = new();
    }

    private SavedUserData CreateSavedData(UserData _data)
    {
        return new()
        {
            //UserName = _data.UserName,
            Diamond = _data.Diamond.ToString(),
            Gold = _data.Gold.ToString(),
            MagicStone = _data.MagicStone.ToString(),
            CurrentLevel = _data.CurrentLevel,
            CurrentExp = _data.CurrentExp.ToString(),
            BigWave = _data.BigWave,
            Wave = _data.Wave,
            MaxHealthLevel = _data.MaxHealthLevel,
            RecoveryLevel = _data.RecoveryLevel,
            AttackLevel = _data.AttackLevel,
            AttackMultiplierLevel = _data.AttackMultiplierLevel,
            AttackRangeLevel = _data.AttackRangeLevel,
            AttackSpeedLevel = _data.AttackSpeedLevel,
            DefenseLevel = _data.DefenseLevel,
            CriticalChanceLevel = _data.CriticalChanceLevel,
            CriticalMultiplierLevel = _data.CriticalMultiplierLevel,
            CounterAttackLevel = _data.CounterAttackLevel,
            SkillsInUseList = JsonConvert.SerializeObject(_data.SkillsInUseList),
            SkillDict = JsonConvert.SerializeObject(_data.SkillDict)
        };
    }

    private void LoadSavedData(UserData _userData, SavedUserData _savedData)
    {
        //_userData.UserName = _savedData.UserName;
        _userData.Diamond = BigInteger.Parse(_savedData.Diamond);
        _userData.Gold = BigInteger.Parse(_savedData.Gold);
        _userData.MagicStone = BigInteger.Parse(_savedData.MagicStone);
        _userData.CurrentLevel = _savedData.CurrentLevel;
        _userData.CurrentExp = BigInteger.Parse(_savedData.CurrentExp);
        _userData.BigWave = _savedData.BigWave;
        _userData.Wave = _savedData.Wave;
        _userData.MaxHealthLevel = _savedData.MaxHealthLevel;
        _userData.RecoveryLevel = _savedData.RecoveryLevel;
        _userData.AttackLevel = _savedData.AttackLevel;
        _userData.AttackMultiplierLevel = _savedData.AttackMultiplierLevel;
        _userData.AttackRangeLevel = _savedData.AttackRangeLevel;
        _userData.AttackSpeedLevel = _savedData.AttackSpeedLevel;
        _userData.DefenseLevel = _savedData.DefenseLevel;
        _userData.CriticalChanceLevel = _savedData.CriticalChanceLevel;
        _userData.CriticalMultiplierLevel = _savedData.CriticalMultiplierLevel;
        _userData.CounterAttackLevel = _savedData.CounterAttackLevel;

        if (_savedData.SkillsInUseList.IsNullOrWhiteSpaceEx() == false)
            _userData.SkillsInUseList = JsonConvert.DeserializeObject<List<int>>(_savedData.SkillsInUseList);

        if (_savedData.SkillDict.IsNullOrWhiteSpaceEx() == false)
            _userData.SkillDict = JsonConvert.DeserializeObject<Dictionary<int, SavedSkillData>>(_savedData.SkillDict);
    }

    private Param CreateParamForBackend(SavedUserData _old, SavedUserData _new)
    {
        Param param = new();

        Type type = typeof(SavedUserData);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).ToList();

        if (_old != null)
        {
            // 기존의 세이브와 신규 세이브를 비교해 달라진 속성에 대해서만 갱신할 수 있도록 선별하여 param을 추가합니다.
            fields.Where(field => !object.Equals(field.GetValue(_old), field.GetValue(_new)))
                .ToList()
                .ForEach(field =>
                {
                    var value = field.GetValue(_new);
                    param.Add(field.Name, value);
                });
        }
        else
        {
            fields.ForEach(field =>
            {
                var value = field.GetValue(_new);
                param.Add(field.Name, value);
            });
        }

        return param;
    }

    private bool FetchOrInsertFromBackend()
    {
        var bro = Backend.GameData.GetMyData(USERDATA_TABLE_NAME, new Where());

        if (bro.IsSuccess())
        {
            if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            {
                // 사용자 데이터가 존재하지 않습니다.
                // 새로운 세이브 데이터를 생성하고 백엔드 userData에 행을 추가합니다.
                CreateUserData(userData);
                savedUserData = CreateSavedData(userData);
                Param param = CreateParamForBackend(null, savedUserData);
                var _insertBro = Backend.GameData.Insert(USERDATA_TABLE_NAME, param);
                if (_insertBro.IsSuccess() == false)
                {
                    Debug.LogError("");
                    return false;
                }

                return true;
            }

            // 세이브 데이터가 존재하는 경우, 불러온 값을 userData에 기록합니다.
            var _json = bro.FlattenRows()[0].ToJson();
            savedUserData = JsonConvert.DeserializeObject<SavedUserData>(_json);
            //savedUserData.UserName = Backend.UserNickName; // 닉네임은 json 데이터에 포함되어 있지 않으므로 따로 불러옵니다.
            LoadSavedData(userData, savedUserData);
            UpdateToBackend(savedUserData); // userData 스키마가 변경된 이후, 게임에 접속한 뒤 바로 반영하도록 갱신합니다.
            return true;
        }

        return false;
    }

    private void UpdateToBackend(SavedUserData _newData)
    {
        // 트랜잭션을 이용하여 한 번에 보냅니다.
        List<TransactionValue> transactionList = new List<TransactionValue>();
        
        var _param = CreateParamForBackend(savedUserData, _newData);
        transactionList.Add(TransactionValue.SetUpdate(USERDATA_TABLE_NAME, new Where(), _param));

        if (GameManager.Instance.questData != null)
        {
            var _questParam = GameManager.Instance.questData.GetChangedParam();
            transactionList.Add(TransactionValue.SetUpdate(QuestData.TABLE_NAME, new Where(), _questParam));
        } 

        var bro = Backend.GameData.TransactionWriteV2(transactionList);

#if UNITY_EDITOR
        if (!bro.IsSuccess())
        {
            Debug.LogError("서버에 업데이트 되지 않았습니다.");
        }
#endif

    }

    private bool FetchOrAddFromLocal()
    {
        string LOCAL_FILE_PATH = Path.Combine(Application.persistentDataPath, "SavedUserData.json");
        if (File.Exists(LOCAL_FILE_PATH) == false)
        {
            File.Create(LOCAL_FILE_PATH).Dispose();
            CreateUserData(userData);
            savedUserData = CreateSavedData(userData);
            UpdateToLocal(savedUserData);
            return true;
        }

        string _json = File.ReadAllText(LOCAL_FILE_PATH);
        savedUserData = JsonConvert.DeserializeObject<SavedUserData>(_json);
        LoadSavedData(userData, savedUserData);
        return true;
    }

    private void UpdateToLocal(SavedUserData _newData)
    {
        string LOCAL_FILE_PATH = Path.Combine(Application.persistentDataPath, "SavedUserData.json");
        string _json = JsonConvert.SerializeObject(_newData);
        File.WriteAllText(LOCAL_FILE_PATH, _json);
    }

    public void GetReward(BigInteger _exp, BigInteger _magicstone, BigInteger _gold, monsterType _monsterType)
    {
        if (_exp > 0)
        {
            userData.CurrentExp += _exp;

            // 현재 경험치가 안 남을 때까지 레밸업을 시도합니다.
            // 최대 레밸 상태이면 레밸업을 시도하지 않습니다.
            while (userData.CurrentExp >= MaxExp && userData.CurrentLevel != PlayerTapParser.Instance.MAX_LEVEL_PLAYER)
            {
                // caution: 연산 순서에 주의하세요.
                userData.CurrentExp -= MaxExp;
                userData.CurrentLevel++;
            }
        }

        if (_magicstone > 0)
        {
            userData.MagicStone += _magicstone;
        }

        // temp: 임시적으로 골드 드랍률을 20%로 설정합니다.
        if (_gold > 0 && Random.Range(0f, 1f) > 0.2f)
        {
            if (_monsterType == monsterType.Basic)
            {
                userData.Gold += _gold;
            }
        }
    }

    public void ModifierCurrencyValue(CurrencyType type, BigInteger value)
    {
        switch (type)
        {
            case CurrencyType.Diamond:
                userData.Diamond += value;
                break;

            case CurrencyType.Gold:
                userData.Gold += value;
                break;

            case CurrencyType.MagicStone:
                userData.MagicStone += value;
                break;
        }
    }
}