using System;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttackTower : MonoBehaviour
{
    [SerializeField]
    Friend friend;

    List<Monster> monsters = new List<Monster>();   

    float originalPlayerHp;

    void OnEnable()
    {
        monsters.Clear();
        originalPlayerHp = GameManager.Instance.playerScript.Stats["Hp"].CurrentValueInt;
    }

    void FixedUpdate()
    {
        if (Math.Abs(originalPlayerHp - GameManager.Instance.playerScript.Stats["Hp"].CurrentValueInt) < 0.01f)
            return;

        originalPlayerHp = GameManager.Instance.playerScript.Stats["Hp"].CurrentValueInt;

        var attackAmount = friend.attackAmount;
        GameManager.Instance.playerScript.counterAttack.CounterAttack_Funtion(attackAmount);
    }
}
        
    
