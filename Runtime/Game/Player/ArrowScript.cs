using System.Collections;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector] public int damage = 70;

    public bool isShield = false;
    private int shieldAmount = 0;

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
            shieldAmount = collision.GetComponent<Shield>().shieldAmount;
            isShield = true;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
