using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class distanceFriend : MonoBehaviour
{
    public int attackAmount { get; protected set; }

    protected float curTime = 0f;
    protected float bulletSpeed = 7f;
    protected float coolTime = 3f;

    [SerializeField] 
    Friend friend;

    private void OnEnable()
    {
        attackAmount = (int)friend.attackAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster") && transform.parent.GetComponent<Friend>().searchObj == null)
        {
            transform.parent.GetComponent<Friend>().searchObj = collision.GetComponent<Monster>();   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && 
            transform.parent.GetComponent<Friend>().searchObj == collision.GetComponent<Monster>())
        {
            transform.parent.GetComponent<Friend>().searchObj = null;
        }
    }

}
