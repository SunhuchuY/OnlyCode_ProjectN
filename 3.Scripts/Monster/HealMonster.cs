using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;

public class HealMonster : MonoBehaviour
{
    private Monster monster;

    [SerializeField] private HealMonsterAround healMonsterAround;
    [SerializeField] private HealMonsterType type = HealMonsterType.hpPercent;
    [SerializeField] private float value = 0;

    private void Awake()
    {
        monster = GetComponent<Monster>();  
    }

    public void AlternativeFuntionHeal()
    {
        switch (type)
        {
            case HealMonsterType.hpPercent:
                float temp = (float)GameManager.Instance.playerScript.GetcurrentHealth() * (value / 100);
                healMonsterAround.healRange(temp);
            break;
        }

    }
}
