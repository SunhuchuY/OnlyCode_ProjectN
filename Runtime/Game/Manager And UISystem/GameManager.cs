using BackEnd;
using Cysharp.Threading.Tasks;
using Story;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public Player playerScript;
    public BulletController bulletController;
    public WaveManager waveManager;
    public SkillManager skillManager;
    public AppearTextManager appearTextManager;
    public AcquireMessageUI acquireMessageUI;
    public SkillTreeManager skillTreeManager;
    private ResourceManager resourceManager;
    public GameplayWorld world;

    [FormerlySerializedAs("particleManager")]
    public ObjectPoolManager objectPoolManager;

    public AudioManager audioManager;
    public UserDataManager userDataManager;
    public CommonUI commonUI;
    public RewardPopupUI rewardPopupUI;
    public LevelUpPopupUI levelUpPopupUI;
    public Transform gachaTf;
    public BossPopup bossPopupUI;
    public GameSettingsPanelUI gameSettingsPanelUI;
    public GameSettingsAccountSettingsPanelUI accountSettingsPanelUI;
    public GameSetting gameSetting;
    public QuestPanelUI questPanelUI;
    public RankingPanelUI rankingPanelUI;
    public UpgradeOwnSkillsPopupUI upgradeOwnSkillsPopup;
    public UseOwnSkillInventory useOwnSkillInventory;
    public Story.StoryManager storyManager;
    public bool isGameStop = false; // 게임 일시정지
    public QuestData questData;

    public List<TransactionValue> TransactionList { get; private set; } = new List<TransactionValue>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;

        userDataManager = new UserDataManager();
        questData = new QuestData();
        resourceManager = new();
        resourceManager.Initialize().Forget();
        world = new GameplayWorld();
        storyManager = new StoryManager();
        GameplayHandlers.Initialize();
    }

    private void OnApplicationQuit()
    {
        userDataManager.Update();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // 앱이 백그라운드로 전환될 때
            userDataManager.Update();
        }
        else
        {
            // 앱이 다시 포그라운드로 전환될 때
        }
    }

    public static bool RandomRange(int start, int end)
    {
        if (Random.Range(start, end) == start)
            return true;
        else
            return false;
    }

    static public bool RandomRange(float start, float end)
    {
        if (Random.Range(start, end) == start)
            return true;
        else
            return false;
    }
}