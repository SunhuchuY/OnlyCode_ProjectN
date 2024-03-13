using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingArrow : MonoBehaviour
{
    private float speed = 2f;
    private Rigidbody2D rb;

    public bool isShield = false;
    public int damage = 100;
    public GameObject effectPrefeb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;

        StartCoroutine(delayDestroy());
    }

    private IEnumerator delayDestroy()
    {
        yield return new WaitForSeconds(7f);
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
       

    }


}
