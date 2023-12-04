using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public GameObject player;
    public Player playerScript;
    public UIManager uIManager;
    public DropManager dropManager;
    public BulletController bulletController;
    public MonsterControll monsterControll;
    public SkillManager skillManager;
    public AppearTextManager appearTextManager;
    public AccuireBoxManager accuireBoxManager;
    public SkillTreeManager skillTreeManager;
    public ParticleManager particleManager;
    public AudioManager audioManager;
    public StatisticScreen statisticScreen;
    public bool isGameStop = false; // 게임 일시정지

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    static public bool RandomRange(int start , int end)
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


