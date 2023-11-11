using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    BulletController _bulletController;
    [SerializeField] MonsterControll _monsterControll;
    [SerializeField] SkillManager _skillManager;
    [SerializeField] DropManager _dropManager;
    [SerializeField] AppearTextManager _appearTextManager;
    [SerializeField] AccuireBoxManager _accuireBoxManager;
    [SerializeField] SkillTreeManager _skillTreeManager;
    [SerializeField] ParticleManager _particleManager;
    [SerializeField] AudioManager _audioManager;
    [SerializeField] Player _playerScript;
    [SerializeField] UIManager _uIManager;
    [SerializeField] StatisticScreen _statisticScreen;


    static public GameObject player;

    static public Player playerScript;
    static public UIManager uIManager;
    static public DropManager dropManager;
    static public BulletController bulletController;
    static public MonsterControll monsterControll;
    static public SkillManager skillManager;
    static public AppearTextManager appearTextManager;
    static public AccuireBoxManager accuireBoxManager;
    static public SkillTreeManager skillTreeManager;
    static public ParticleManager particleManager;
    static public AudioManager audioManager;
    static public StatisticScreen statisticScreen;
    static public bool isGameStop = false; // 게임 일시정지

    private void Awake()
    {
        player = _player;
        playerScript = _playerScript;
        monsterControll = _monsterControll;
        uIManager = _uIManager;
        dropManager = _dropManager;
        bulletController = _bulletController;
        skillManager = _skillManager;
        appearTextManager = _appearTextManager;
        accuireBoxManager = _accuireBoxManager;
        skillTreeManager = _skillTreeManager;
        particleManager = _particleManager;
        audioManager = _audioManager;
        statisticScreen = _statisticScreen; 
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


