using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterControll : MonoBehaviour
{
    public GameObject[] monsterPrefab; // 몬스터 프리팹
    public float spawnRate = 3.0f; // 몬스터 생성 간격 (초)
    public float spawnXMin = -8.0f; // 몬스터가 생성될 X 최소 위치
    public float spawnXMax = 8.0f; // 몬스터가 생성될 X 최대 위치
    public float spawnYMin = -4.0f; // 몬스터가 생성될 Y 최소 위치
    public float spawnYMax = 4.0f; // 몬스터가 생성될 Y 최대 위치

    // wave //
    public int[] waveFrom;
    public int[] waveUntil;
    public int[] bigWave_numofcatchs;

    public string[] _monsterStr; // 예를 들어서 0, 1 이런식으로 입력하면 monsterPrefab[0] , monsterPrefab[1]을 가져오도록 설계
    private List<int> monsterTempIndexList; // 임시 저장소

    public Image waveBar;
    private Transform waveBar_Parent;
    [SerializeField] TMP_Text curWaveLev_Text, bossWave_Text,waveProgress_Text;
    [SerializeField] Transform bossBar_position;

    public float nuberofCatchs = 0;
    public int curWaveLev = 1, index = 1;
    float nuberofCatchsMax = 10000f;

    const float waveanimDuration = 0.2f;

    public bool isBoss = false;
    private Vector3 originalBarParent_Scale;
    private Vector3 originalBarParent_Position;
    // wave //


    [SerializeField] Transform list_MonsterParent;

    private float nextSpawnTime = 5f;

    bool isInstant_Stop = false;

    // current spawn monsters
    List<Monster> spawnMonsters = new List<Monster>();  

    // monster object pooling 
    List<GameObject> parents = new List<GameObject>();
    [SerializeField] GameObject emptyObject;
    const int Mount = 10;

    // boss explain panel
    [SerializeField] GameObject bossPopup;
    [SerializeField] TMP_Text bossExplain_Text;
    [SerializeField] string[] bossExplain_String;

    public Transform Getlist_MonsterParent()
    {
        return list_MonsterParent;
    }

    private void Start()
    {
        waveBar_Parent = waveBar.transform.parent;
        originalBarParent_Scale = waveBar_Parent.localScale;
        originalBarParent_Position = waveBar_Parent.transform.position;
        bossWave_Text.gameObject.SetActive(false);
        bossPopup.SetActive(false);

        for (int k = 0; k < monsterPrefab.Length; k++)
        {

            GameObject _parent = Instantiate(emptyObject);
            parents.Add(_parent);
            _parent.transform.parent = list_MonsterParent.transform;

            if (monsterPrefab[k] == null)
                continue;

            for (int i = 0; i < Mount; i++)
            {
                GameObject instant = Instantiate(monsterPrefab[k]);
                instant.transform.parent = _parent.transform;
                instant.SetActive(false);
            }
        }

        BigWaveSet();

       
    }

    private void Update()
    {
        // 현재 시간이 다음 몬스터 생성 시간을 지났다면 몬스터를 생성합니다.
        if (Time.time >= nextSpawnTime && list_MonsterParent.gameObject.activeSelf && spawnNumCount < nuberofCatchsMax)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnRate;
        }

        SmallWaveSet();
    }

    private void SpawnMonster()
    {
        if (monsterTempIndexList.Count == 0)
            return;

        // 몬스터를 생성할 위치를 계산합니다.
        Vector3 spawnPosition = CalculateSpawnPosition();

        // 몬스터를 생성하고 위치를 설정합니다.
        int rand = Random.Range(0, monsterTempIndexList.Count - 1);
        int toIndex = monsterTempIndexList[rand];
        GameObject obj = null;
        Monster objMoster = null;


        for (int i = 0; i < Mount; i++)
        {
            if (parents[toIndex].transform.GetChild(i).gameObject.activeSelf == false)
            {
                // monster spawn setting
                obj = parents[toIndex].transform.GetChild(i).gameObject;
                objMoster = obj.GetComponent<Monster>();
                obj.SetActive(true);
                obj.transform.position = spawnPosition;

                spawnMonsters.Add(obj.GetComponent<Monster>());

                break;
            }
        }

        if (obj == null)
            return;

        spawnNumCount++;
    }

    private Vector3 CalculateSpawnPosition()
    {
        float spawnX, spawnY;

        // 랜덤하게 X와 Y 위치를 선택합니다.
        float randomDirection = Random.Range(0f, 1f);

        if (randomDirection < 0.25f) // 위
        {
            spawnX = Random.Range(spawnXMin, spawnXMax);
            spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 10)).y; // 10은 Z 위치, 화면 위쪽 끝을 나타냅니다.
        }
        else if (randomDirection < 0.5f) // 오른쪽
        {
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 10)).x; // 10은 Z 위치, 화면 오른쪽 끝을 나타냅니다.
            spawnY = Random.Range(spawnYMin, spawnYMax);
        }
        else if (randomDirection < 0.75f) // 왼쪽
        {
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 10)).x; // 10은 Z 위치, 화면 왼쪽 끝을 나타냅니다.
            spawnY = Random.Range(spawnYMin, spawnYMax);
        }
        else // 아래
        {
            spawnX = Random.Range(spawnXMin, spawnXMax);
            spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 10)).y; // 10은 Z 위치, 화면 아래쪽 끝을 나타냅니다.
        }

        return new Vector3(spawnX, spawnY, 0);
    }

    private int GetBigWaveIndex()
    {
        for (int i = 0; i < waveFrom.Length; i++)
        {
            // check the BIg wave index
            if (waveFrom[i] <= curWaveLev && curWaveLev <= waveUntil[i])
                return i;
        }

        // anyway error : Occur outofrange
        return -1;
    }

    public void BigWaveSet(PlayerStateEnum playerStateEnum = PlayerStateEnum.NULL)
    {
        for (int j = 0; j < spawnMonsters.Count; j++)
            spawnMonsters[j].gameObject.SetActive(false);

        spawnMonsters.Clear();

        if (monsterTempIndexList != null)
            monsterTempIndexList.Clear();

        // when player dead and restart bigwave
        if (playerStateEnum == PlayerStateEnum.Restart)
        {
            curWaveLev = waveFrom[GetBigWaveIndex()];
        }

        for (int i = 0; i < waveFrom.Length; i++)
        {
            if(curWaveLev == waveFrom[i])
            {
                monsterTempIndexList = ExtractNumbers(_monsterStr[i]);
                nuberofCatchsMax = bigWave_numofcatchs[i];
                nuberofCatchs = 0;
                spawnNumCount = 0;
                index = i;

                isBoss = false;
                return;
            }
        }
    }


    Monster tempBossMonster;
    float apparance_duration = 1f;
    int spawnNumCount = 0;

    // update funtion
    public void SmallWaveSet() // 보스몬스터있는지 체크해야함
    {
        curWaveLev_Text.text = $"Wave{curWaveLev}";

        if (isBoss == true)
            return;

        if (nuberofCatchsMax <= nuberofCatchs) // small wave 종료
        {
            nuberofCatchs = 0;
            curWaveLev++;
            spawnNumCount = 0;
            Debug.Log(nuberofCatchsMax);
        }

        if (curWaveLev == waveUntil[index]) // 보스출현
        {
            Vector3 spawnPosition = CalculateSpawnPosition();

            int temp = monsterTempIndexList[monsterTempIndexList.Count-1];
            GameObject bossObj;

            for (int i = 0; i < Mount; i++)
            {
                if (parents[temp].transform.GetChild(i).gameObject.activeSelf == false)
                {
                    bossObj = parents[temp].transform.GetChild(i).gameObject;
                    bossObj.SetActive(true);
                    bossObj.transform.position = spawnPosition;

                    tempBossMonster = bossObj.GetComponent<Monster>();
                    break;
                }
            }
            
            BossStart(index);

            isBoss = true;
        }

        if (isBoss) // 보스일때 바 상태
        {
            waveBar.transform.DOScaleX(tempBossMonster.Health.CurrentValue / tempBossMonster.maxHealth, waveanimDuration);
        }
        else
        {
            waveBar.transform.DOScaleX(nuberofCatchs / nuberofCatchsMax, waveanimDuration);
            waveProgress_Text.text = $"{nuberofCatchs}/{nuberofCatchsMax}";
        }

    }

    IEnumerator BossStart(int index)
    {
        const float delay = 4f;

        curWaveLev_Text.gameObject.SetActive(false);
        bossWave_Text.gameObject.SetActive(true);

        bossPopup.SetActive(true);
        yield return null;
    }

    public void BossExit()
    {
        curWaveLev_Text.gameObject.SetActive(true);
        bossWave_Text.gameObject.SetActive(false);

    }

    List<int> ExtractNumbers(string input)
    {
        List<int> numbers = new List<int>();

        // 정규식을 사용하여 중괄호 안의 숫자 추출
        MatchCollection matches = Regex.Matches(input, @"\{(\d+)\}");

        // 각 매치에서 숫자를 추출하여 리스트에 추가
        foreach (Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                string numberStr = match.Groups[1].Value;
                int number;
                if (int.TryParse(numberStr, out number))
                {
                    numbers.Add(number);
                }
            }
        }

        return numbers;
    }
}
