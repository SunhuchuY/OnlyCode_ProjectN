using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceMonster : MonoBehaviour
{

    const string isAttack = "isAttack";

    [SerializeField] GameObject arrow;  
    
    Monster parentMonster;

    private void Start()
    {
        parentMonster = transform.parent.GetComponent<Monster>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                if (parentMonster.targetObj!= null && parentMonster.targetObj.gameObject.activeSelf == false)
                {
                    parentMonster.targetObj = null;
                }

                parentMonster.targetObj = collision.transform;
                parentMonster.OnAnimator(isAttack);
                break;
        }
    }
}
