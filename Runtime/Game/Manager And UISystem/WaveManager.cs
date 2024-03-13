using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class WaveManager : MonoBehaviour
{
    private float spawnInterval = 3f;
    public WaveProfile CurrentBigWaveData => StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave];
    public int MaxSpawnCountInCurrentWave =>
        isBossWave ? 1 : StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].BigWaveNumOfCatches;

    public int KillCountInCurrentWave { get; private set; }

    public event System.Action OnWaveIndexChanged;
    public event System.Action OnBigWaveIndexChanged;
    public event System.Action<Monster> OnMonsterSpawned;

    private int spawnCountInCurrentWave = 0;
    private bool isBossWave = false;
    private Coroutine waveCoroutine;

    private float spawnXMin; // 몬스터가 생성될 X 최소 위치
    private float spawnXMax; // 몬스터가 생성될 X 최대 위치
    private float spawnYMin; // 몬스터가 생성될 Y 최소 위치
    private float spawnYMax; // 몬스터가 생성될 Y 최대 위치

    private Monster currentBoss;

    public void Start()
    {
        InitSpawnPointMinMax();

        OnBigWaveIndexChanged?.Invoke();
        OnWaveIndexChanged?.Invoke();

        StartNextWave();
    }

    private void UpdateWave()
    {
        if (KillCountInCurrentWave >= MaxSpawnCountInCurrentWave)
        {
            // 현재 wave의 몬스터를 모두 죽였다면 다음 wave로 넘어갑니다.
            KillCountInCurrentWave = 0;
            spawnCountInCurrentWave = 0;
            ++GameManager.Instance.userDataManager.userData.Wave;
            OnWaveIndexChanged?.Invoke();

            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        if (waveCoroutine != null)
        {
            // 새로운 wave가 시작되었으므로, 이전 wave의 코루틴을 중지합니다.
            StopCoroutine(waveCoroutine);
        }

        int _nextBigWaveBegin = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave + 1].WaveFrom;

        if (GameManager.Instance.userDataManager.userData.Wave == _nextBigWaveBegin - 1)
        {
            // 다음 big wave 바로 이전의 wave는 보스전입니다.
            waveCoroutine = StartCoroutine(StartBossWaveCoroutine());
        }
        else
        {
            if (GameManager.Instance.userDataManager.userData.Wave == _nextBigWaveBegin)
            {
                // 다음 big wave에 진입했을 때 실행됩니다.
                ++GameManager.Instance.userDataManager.userData.BigWave;
                OnBigWaveIndexChanged?.Invoke();

                GameManager.Instance.userDataManager.Update();
            }

            waveCoroutine = StartCoroutine(StartNormalWaveCoroutine());
        }
    }

    private Vector2 GetRandomSpawnPoint()
    {
        float _spawnX;
        float _spawnY;

        int _direction = Random.Range(0, 4);
        if (_direction == 0) // 위
        {
            _spawnX = Random.Range(spawnXMin, spawnXMax);
            _spawnY = spawnYMax;
        }
        else if (_direction == 1) // 아래
        {
            _spawnX = Random.Range(spawnXMin, spawnXMax);
            _spawnY = spawnYMin;
        }
        else if (_direction == 2) // 오른쪽
        {
            _spawnX = spawnXMax;
            _spawnY = Random.Range(spawnYMin, spawnYMax);
        }
        else // 왼쪽
        {
            _spawnX = spawnXMin;
            _spawnY = Random.Range(spawnYMin, spawnYMax);
        }

        return new Vector2(_spawnX, _spawnY);
    }

    private void SpawnRandomMonster()
    {
        int _rand = Random.Range(0, StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].MonsterIDs.Count - 1);
        int _id = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].MonsterIDs[_rand];

        SpawnMonster(_id);
    }

    private Monster SpawnMonster(int _id)
    {
        var _actor = GameManager.Instance.world.SpawnActor(StageParser.Instance.monsterProfiles[_id]);
        _actor.Go.transform.position = GetRandomSpawnPoint();

        var _monster = _actor as Monster;
        // 몬스터가 죽었을 때 wave index 갱신을 시도합니다.
        _monster.keyframeEventReceiver.OnDeadEvent -= UpdateDeadMonster;
        _monster.keyframeEventReceiver.OnDeadEvent += UpdateDeadMonster;

        ++spawnCountInCurrentWave;
        OnMonsterSpawned?.Invoke(_monster);

        return _monster;
    }

    public void UpdateDeadMonster()
    {
        ++KillCountInCurrentWave;
        UpdateWave();
        
        if (BackEnd.Backend.IsLogin)
        {
            GameManager.Instance.questData.questData.DailyKillCount.Value++; 
            GameManager.Instance.questData.questData.WeekKillCount.Value++; 
        }
    }

    private void InitSpawnPointMinMax()
    {
        Vector2 _leftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f, 0));
        Vector2 _rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

        spawnXMin = _leftBottom.x;
        spawnYMin = _leftBottom.y;
        spawnXMax = _rightTop.x;
        spawnYMax = _rightTop.y;
    }

    public void Restart()
    {
        // 출현한 모든 몬스터를 삭제합니다.
        GameManager.Instance.world.Actors
            .Where(actor => actor.ActorType == ActorType.Monster)
            .Select(actor => actor)
            .ToList()
            .ForEach(actor => Destroy(actor.Go));

        // 플레이어가 죽어서 재시작될 때에는 wave index를 big wave가 시작될 때의 index로 초기화합니다.
        GameManager.Instance.userDataManager.userData.Wave = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].WaveFrom;
        KillCountInCurrentWave = 0;
        spawnCountInCurrentWave = 0;
        StartNextWave();
        OnWaveIndexChanged?.Invoke();
    }

    /// <summary>
    /// 스탯이 모두 초기화 되어야 하기 떄문에 두 프레임을 쉬고 넣습니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddRankBoss_OnCurrentValueIsZeroOrLess(Monster boss) 
    {
        yield return null;
        yield return null;

        if (BackEnd.Backend.IsLogin)
        {
            Attribute hp = boss.Stats["Hp"] as Attribute;
            hp.OnCurrentValueIsZeroOrLess += currentValue =>
            {
                BackEnd.Rank.Rank.AddValueMyRank(boss.Stats["Hp"].Cap.Value - currentValue);
            };
        }
    }

    private IEnumerator StartBossWaveCoroutine()
    {
        isBossWave = true;

        GameManager.Instance.bossPopupUI.Show();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.bossPopupUI.Hide();

        // 마지막 인덱스는 보스몬스터입니다.
        int _id = StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].
            MonsterIDs[StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].MonsterIDs.Count - 1];
        
        Monster boss = SpawnMonster(_id);
        StartCoroutine(AddRankBoss_OnCurrentValueIsZeroOrLess(boss));
    }

    private IEnumerator StartNormalWaveCoroutine()
    {
        isBossWave = false;

        while (true)
        {
            if (spawnCountInCurrentWave >= StageParser.Instance.bigWaveProfiles[GameManager.Instance.userDataManager.userData.BigWave].BigWaveNumOfCatches)
            {
                // 이번 wave에 스폰할 몬스터를 모두 스폰했다면 더 이상 스폰하지 않습니다.
                break;
            }

            // 몬스터를 스폰한 뒤에는 spawn interval만큼 대기합니다.
            SpawnRandomMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}