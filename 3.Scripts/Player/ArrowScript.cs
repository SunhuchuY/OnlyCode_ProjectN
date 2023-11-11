using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector] public int damage = 70;

    public bool isShield = false;

    private void OnEnable()
    {
        StartCoroutine(DestroyCoroutine()); 
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            isShield = true;
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isShield)
        {
            if (collision.GetComponent<Player>() != null)
                GameManager.playerScript.GetDamage(damage);
            else if (collision.GetComponent<Friend>() != null)
                collision.GetComponent<Friend>().GetDamage(damage);

            Destroy(gameObject);

        }

    }


}
