using Cysharp.Threading.Tasks;
using System.Numerics;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfileAndProgressPanelUI : MonoBehaviour
{
    [SerializeField] private ProfileAndProgressPanelAccessor accessor;

    private Tweener experienceTweener;
    private Tweener monsterKillCountTweener;

    private void Start()
    {
        //GameManager.Instance.userDataManager.userData
        //    .ObserveEveryValueChanged(x => x.UserName)
        //    .Subscribe(_ => UpdateUsername())
        //    .AddTo(gameObject);
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CurrentLevel)
            .Subscribe(_ => UpdateCurrentLevel())
            .AddTo(gameObject);
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CurrentExp)
            .Subscribe(UpdateCurrentExp)
            .AddTo(gameObject);
        GameManager.Instance.waveManager.OnWaveIndexChanged += UpdateWaveIndex;
        GameManager.Instance.waveManager.OnMonsterSpawned += OnMonsterSpawned;

        UpdateUsername();
        UpdateCurrentLevel();
        UpdateCurrentExp(0);
        UpdateWaveIndex();
        UpdateMonsterKillCount();
    }

    private void UpdateUsername()
    {
        //accessor.UsernameText.text = GameManager.Instance.userDataManager.userData.UserName;
    }

    private void UpdateCurrentLevel()
    {
        accessor.LevelText.text = GameManager.Instance.userDataManager.userData.CurrentLevel == PlayerTapParser.Instance.MAX_LEVEL_PLAYER ?
            "Lv.MAX" : $"Lv.{GameManager.Instance.userDataManager.userData.CurrentLevel}";
        UpdateCurrentExp(0);
    }

    private void UpdateCurrentExp(BigInteger _amount)
    {
        int _currentLevel = GameManager.Instance.userDataManager.userData.CurrentLevel;
        if (_currentLevel == PlayerTapParser.Instance.MAX_LEVEL_PLAYER)
        {
            accessor.ExperienceSlider.value = 1;
            accessor.ExperienceText.text = string.Empty;
            return;
        }

        if (experienceTweener != null && experienceTweener.IsActive())
        {
            experienceTweener.Kill();
            experienceTweener = null;
        }

        BigInteger _currentLevelMaxExp = GameManager.Instance.userDataManager.MaxExp;
        BigInteger _currentExp = GameManager.Instance.userDataManager.userData.CurrentExp;

        float _ratio = (float)_currentExp / (float)_currentLevelMaxExp;

        accessor.ExperienceText.text =
            $"{CurrencyHelper.ToCurrencyString(_currentExp)}/{CurrencyHelper.ToCurrencyString(_currentLevelMaxExp)}";
        experienceTweener = accessor.ExperienceSlider.DOValue(_ratio, 0.5f);
    }

    private void UpdateWaveIndex()
    {
        accessor.WaveText.text = $"Wave {GameManager.Instance.userDataManager.userData.Wave}";
        UpdateMonsterKillCount();
    }

    private void UpdateMonsterKillCount()
    {
        if (monsterKillCountTweener != null && monsterKillCountTweener.IsActive())
        {
            monsterKillCountTweener.Kill();
            monsterKillCountTweener = null;
        }

        float _ratio = (float)GameManager.Instance.waveManager.KillCountInCurrentWave /
                       (float)GameManager.Instance.waveManager.CurrentBigWaveData.BigWaveNumOfCatches;

        accessor.MonsterKillCountText.text =
            $"{GameManager.Instance.waveManager.KillCountInCurrentWave}/{GameManager.Instance.waveManager.CurrentBigWaveData.BigWaveNumOfCatches}";
        monsterKillCountTweener = accessor.MonsterKillCountSlider.DOValue(_ratio, 0.5f);
    }

    private void OnMonsterSpawned(Monster _monster)
    {
        _monster.keyframeEventReceiver.OnDeadEvent += UpdateMonsterKillCount;
    }
}