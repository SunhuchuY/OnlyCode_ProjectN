using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideTowerSkill : MonoBehaviour
{
    float curTime = 0, delayTime = 1f;

    private void OnDisable()
    {
        curTime = 0;
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            curTime += Time.deltaTime;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        // 딜레이간격으로 데미지를 가함


        if(collision.CompareTag("Monster") && curTime > delayTime)
        {
             collision.GetComponent<Monster>().GetDamage(transform.parent.GetComponent<distanceFriend>().attackAmount, true);
             curTime = 0;
        }
    }
}
