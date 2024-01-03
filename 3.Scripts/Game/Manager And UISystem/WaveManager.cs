using System.Collections;
using UnityEngine;

[System.Serializable]
public class BigWaveData
{
    public int WaveCount;
    public int MonsterCount;
    public int[] MonsterIds;
    public int BossMonsterId;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefab; // 몬스터 프리팹
    [SerializeField] private BigWaveData[] bigWaveDatas;
    private float spawnInterval = 3f;

    public BigWaveData CurrentBigWaveData => bigWaveDatas[BigWaveIndex];

    public int MaxSpawnCountInCurrentWave =>
        isBossWave ? 1 : CurrentBigWaveData.MonsterCount;

    public int WaveIndex { get; private set; }
    public int BigWaveIndex { get; private set; }
    public int KillCountInCurrentWave { get; private set; }

    public event System.Action OnWaveIndexChanged;
    public event System.Action OnBigWaveIndexChanged;
    public event System.Action<Monster> OnMonsterSpawned;

    private int[] beginBigWaveIndex;
    private int spawnCountInCurrentWave = 0;
    private bool isBossWave = false;
    private Coroutine waveCoroutine;

    private float spawnXMin; // 몬스터가 생성될 X 최소 위치
    private float spawnXMax; // 몬스터가 생성될 X 최대 위치
    private float spawnYMin; // 몬스터가 생성될 Y 최소 위치
    private float spawnYMax; // 몬스터가 생성될 Y 최대 위치

    private void Start()
    {
        InitSpawnPointMinMax();
        InitBeginBigWaveIndex();

        StartNextWave();
    }

    private void UpdateWaveIndex()
    {
        if (KillCountInCurrentWave >= MaxSpawnCountInCurrentWave)
        {
            // 현재 wave의 몬스터를 모두 죽였다면 다음 wave로 넘어갑니다.
            KillCountInCurrentWave = 0;
            spawnCountInCurrentWave = 0;
            ++WaveIndex;
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

        int _nextBigWaveBeginIndex = beginBigWaveIndex[BigWaveIndex + 1];

        if (WaveIndex == _nextBigWaveBeginIndex - 1)
        {
            // 다음 big wave 바로 이전의 wave는 보스전입니다.
            waveCoroutine = StartCoroutine(StartBossWaveCoroutine());
        }
        else
        {
            if (WaveIndex == _nextBigWaveBeginIndex)
            {
                // 다음 big wave에 진입했을 때 실행됩니다.
                ++BigWaveIndex;
                OnBigWaveIndexChanged?.Invoke();
            }

            waveCoroutine = StartCoroutine(StartNormalWaveCoroutine());
        }
    }

    private int GetBigWaveIndex()
    {
        for (int i = 0; i < beginBigWaveIndex.Length; i++)
        {
            if (WaveIndex <= beginBigWaveIndex[i])
                return i;
        }

        // anyway error : Occur OutOfRange
        return -1;
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
        int _rand = Random.Range(0, bigWaveDatas[BigWaveIndex].MonsterIds.Length - 1);
        int _id = bigWaveDatas[BigWaveIndex].MonsterIds[_rand];

        if (_id >= monsterPrefab.Length)
        {
            // temp
            Debug.LogWarning("모든 몬스터 프리팹에 대한 레퍼런스를 설정해두지 않았으므로, 레퍼런스된 몬스터 중 임의의 몬스터로 대체되어 스폰합니다.");
            _id = Random.Range(0, monsterPrefab.Length);
        }

        SpawnMonster(_id);
    }

    private void SpawnMonster(int _id)
    {
        GameObject _monsterGo = Instantiate(monsterPrefab[_id], gameObject.transform);
        _monsterGo.transform.position = GetRandomSpawnPoint();

        var _monster = _monsterGo.GetComponent<Monster>();
        _monster.Health.OnCurrentValueIsZero += () =>
        {
            // 몬스터가 죽었을 때 wave index 갱신을 시도합니다.
            ++KillCountInCurrentWave;
            UpdateWaveIndex();
        };

        ++spawnCountInCurrentWave;
        OnMonsterSpawned?.Invoke(_monster);
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

    private void InitBeginBigWaveIndex()
    {
        beginBigWaveIndex = new int[bigWaveDatas.Length];

        int _sum = 0;
        for (int i = 1; i < bigWaveDatas.Length; i++)
        {
            _sum += bigWaveDatas[i - 1].WaveCount;
            beginBigWaveIndex[i] = _sum;
        }
    }

    public void Restart()
    {
        // 플레이어가 죽어서 재시작될 때에는 wave index를 big wave가 시작될 때의 index로 초기화합니다.
        WaveIndex = beginBigWaveIndex[GetBigWaveIndex()];
        OnWaveIndexChanged?.Invoke();
    }

    private IEnumerator StartBossWaveCoroutine()
    {
        isBossWave = true;

        GameManager.Instance.bossPopupUI.Show();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.bossPopupUI.Hide();

        int _id = bigWaveDatas[BigWaveIndex].BossMonsterId;
        SpawnMonster(_id);
    }

    private IEnumerator StartNormalWaveCoroutine()
    {
        isBossWave = false;

        while (true)
        {
            if (spawnCountInCurrentWave >= bigWaveDatas[BigWaveIndex].MonsterCount)
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