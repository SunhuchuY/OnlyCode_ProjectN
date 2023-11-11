using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    bool firstStart = true;
    Monster monster;

    private void Start()
    {
        monster = GetComponent<Monster>();  
    }

    private void OnDisable()
    {
        if (firstStart == true)
            return;

        if (monster.currentHealth > 0)
            return;

        GameManager.monsterControll.BossExit();
        GameManager.monsterControll.curWaveLev++;
        GameManager.monsterControll.BigWaveSet();
        
        firstStart = false;
    }


}

