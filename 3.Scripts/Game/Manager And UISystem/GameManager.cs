using UnityEngine;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("particleManager")] public ObjectPoolManager objectPoolManager;
    public AudioManager audioManager;
    public StatisticScreen statisticScreen;
    public UserDataManager userDataManager;
    public CommonUI commonUI;
    public ShopPanelUI shopPanelUI;
    public BossPopup bossPopupUI;
    public GameSettingsPanelUI gameSettingsPanelUI;
    public GameSettingsAccountSettingsPanelUI accountSettingsPanelUI;
    public GameSetting gameSetting;
    public bool isGameStop = false; // 게임 일시정지

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        userDataManager = new UserDataManager();
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