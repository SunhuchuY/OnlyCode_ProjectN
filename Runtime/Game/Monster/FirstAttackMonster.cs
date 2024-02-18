using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAttackMonster : MonoBehaviour
{
    private Monster this_monster;

    [HideInInspector] public int firstAttack_Mount { get; internal set; }
    [HideInInspector] public bool isAlreadyFirstAttack { get; internal set; } = false;


    [SerializeField] private float multyplyAttackAmount = 1;    


    private void Start()
    {
        this_monster = GetComponent<Monster>();
    }

    private void Update()
    {
        //firstAttack_Mount = (int)(this_monster.attackMount * multyplyAttackAmount);

    }

}
