using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingArrow : MonoBehaviour
{
    float speed = 2f;
    Rigidbody2D rb;

    public GameObject effectPrefeb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        StartCoroutine(delayDestroy());
    }

    IEnumerator delayDestroy()
    {
        yield return new WaitForSeconds(7f);
        Destroy(gameObject);
    }

    public int damage = 100;

    public bool isShield = false;

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

            Instantiate(effectPrefeb, transform.position, Quaternion.identity);

            Destroy(gameObject);

        }

    }


}
