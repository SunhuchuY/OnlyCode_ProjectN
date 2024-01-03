using System.Numerics;
using DG.Tweening;
using UnityEngine;

public class ProfileAndProgressPanelUI : MonoBehaviour
{
    [SerializeField] private ProfileAndProgressPanelAccessor accessor;

    private Tweener experienceTweener;
    private Tweener monsterKillCountTweener;

    private void Start()
    {
        GameManager.Instance.userDataManager.OnUsernameChanged += UpdateUsername;
        GameManager.Instance.userDataManager.OnAvatarIdChanged += UpdateAvatar;
        GameManager.Instance.userDataManager.OnCurrentLevelChanged += UpdateCurrentLevel;
        GameManager.Instance.userDataManager.OnCurrentExpChanged += UpdateCurrentExp;
        GameManager.Instance.waveManager.OnWaveIndexChanged += UpdateWaveIndex;
        GameManager.Instance.waveManager.OnMonsterSpawned += OnMonsterSpawned;

        UpdateUsername();
        UpdateAvatar();
        UpdateCurrentLevel();
        UpdateCurrentExp(0);
        UpdateWaveIndex();
        UpdateMonsterKillCount();
    }

    private void UpdateUsername()
    {
        accessor.UsernameText.text = GameManager.Instance.userDataManager.Username;
    }

    private void UpdateAvatar()
    {
        accessor.AvatarIcon.sprite =
            Resources.Load<Sprite>($"Sprite/Icon/Avatar/avatar{GameManager.Instance.userDataManager.AvatarId}");
    }

    private void UpdateCurrentLevel()
    {
        accessor.LevelText.text = $"Lv.{GameManager.Instance.userDataManager.CurrentLevel}";
        UpdateCurrentExp(0);
    }

    private void UpdateCurrentExp(BigInteger _amount)
    {
        if (experienceTweener != null && experienceTweener.IsActive())
        {
            experienceTweener.Kill();
            experienceTweener = null;
        }

        int _currentLevel = GameManager.Instance.userDataManager.CurrentLevel;

        BigInteger _currentLevelMaxExpMapped;
        BigInteger _currentExpMapped;

        if (_currentLevel == 1)
        {
            _currentLevelMaxExpMapped = UserDataManager.MAX_EXP_LIST[_currentLevel - 1];
            _currentExpMapped = GameManager.Instance.userDataManager.CurrentExp;
        }
        else
        {
            BigInteger _prevLevelMaxExp = UserDataManager.MAX_EXP_LIST[_currentLevel - 2];

            _currentLevelMaxExpMapped =
                GameManager.Instance.userDataManager.CurrentLevel == 1
                    ? UserDataManager.MAX_EXP_LIST[_currentLevel - 1]
                    : UserDataManager.MAX_EXP_LIST[_currentLevel - 1] - _prevLevelMaxExp;

            _currentExpMapped =
                GameManager.Instance.userDataManager.CurrentLevel == 1
                    ? GameManager.Instance.userDataManager.CurrentExp
                    : GameManager.Instance.userDataManager.CurrentExp - _prevLevelMaxExp;
        }

        float _ratio = (float)_currentExpMapped / (float)_currentLevelMaxExpMapped;

        accessor.ExperienceText.text =
            $"{GameManager.Instance.userDataManager.CurrentExp}/{GameManager.Instance.userDataManager.MaxExp}";
        experienceTweener = accessor.ExperienceSlider.DOValue(_ratio, 0.5f);
    }

    private void UpdateWaveIndex()
    {
        accessor.WaveText.text = $"Wave {GameManager.Instance.waveManager.WaveIndex + 1}";
    }

    private void UpdateMonsterKillCount()
    {
        if (monsterKillCountTweener != null && monsterKillCountTweener.IsActive())
        {
            monsterKillCountTweener.Kill();
            monsterKillCountTweener = null;
        }

        float _ratio = (float)GameManager.Instance.waveManager.KillCountInCurrentWave /
                       (float)GameManager.Instance.waveManager.CurrentBigWaveData.MonsterCount;

        accessor.MonsterKillCountText.text =
            $"{GameManager.Instance.waveManager.KillCountInCurrentWave}/{GameManager.Instance.waveManager.CurrentBigWaveData.MonsterCount}";
        monsterKillCountTweener = accessor.MonsterKillCountSlider.DOValue(_ratio, 0.5f);
    }

    private void OnMonsterSpawned(Monster _monster)
    {
        _monster.Health.OnCurrentValueIsZero += UpdateMonsterKillCount;
    }
}