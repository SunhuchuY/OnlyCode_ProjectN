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
    public GameObject[] monsterPrefab; // ���� ������
    public float spawnRate = 3.0f; // ���� ���� ���� (��)
    public float spawnXMin = -8.0f; // ���Ͱ� ������ X �ּ� ��ġ
    public float spawnXMax = 8.0f; // ���Ͱ� ������ X �ִ� ��ġ
    public float spawnYMin = -4.0f; // ���Ͱ� ������ Y �ּ� ��ġ
    public float spawnYMax = 4.0f; // ���Ͱ� ������ Y �ִ� ��ġ

    // wave //
    public int[] waveFrom;
    public int[] waveUntil;
    public int[] bigWave_numofcatchs;

    public string[] _monsterStr; // ���� �� 0, 1 �̷������� �Է��ϸ� monsterPrefab[0] , monsterPrefab[1]�� ���������� ����
    private List<int> monsterTempIndexList; // �ӽ� �����

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
        // ���� �ð��� ���� ���� ���� �ð��� �����ٸ� ���͸� �����մϴ�.
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

        // ���͸� ������ ��ġ�� ����մϴ�.
        Vector3 spawnPosition = CalculateSpawnPosition();

        // ���͸� �����ϰ� ��ġ�� �����մϴ�.
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

        // �����ϰ� X�� Y ��ġ�� �����մϴ�.
        float randomDirection = Random.Range(0f, 1f);

        if (randomDirection < 0.25f) // ��
        {
            spawnX = Random.Range(spawnXMin, spawnXMax);
            spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 10)).y; // 10�� Z ��ġ, ȭ�� ���� ���� ��Ÿ���ϴ�.
        }
        else if (randomDirection < 0.5f) // ������
        {
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 10)).x; // 10�� Z ��ġ, ȭ�� ������ ���� ��Ÿ���ϴ�.
            spawnY = Random.Range(spawnYMin, spawnYMax);
        }
        else if (randomDirection < 0.75f) // ����
        {
            spawnX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 10)).x; // 10�� Z ��ġ, ȭ�� ���� ���� ��Ÿ���ϴ�.
            spawnY = Random.Range(spawnYMin, spawnYMax);
        }
        else // �Ʒ�
        {
            spawnX = Random.Range(spawnXMin, spawnXMax);
            spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 10)).y; // 10�� Z ��ġ, ȭ�� �Ʒ��� ���� ��Ÿ���ϴ�.
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
    public void SmallWaveSet() // ���������ִ��� üũ�ؾ���
    {
        curWaveLev_Text.text = $"Wave{curWaveLev}";

        if (isBoss == true)
            return;

        if (nuberofCatchsMax <= nuberofCatchs) // small wave ����
        {
            nuberofCatchs = 0;
            curWaveLev++;
            spawnNumCount = 0;
            Debug.Log(nuberofCatchsMax);
        }

        if (curWaveLev == waveUntil[index]) // ��������
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

        if (isBoss) // �����϶� �� ����
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

        // ���Խ��� ����Ͽ� �߰�ȣ ���� ���� ����
        MatchCollection matches = Regex.Matches(input, @"\{(\d+)\}");

        // �� ��ġ���� ���ڸ� �����Ͽ� ����Ʈ�� �߰�
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
