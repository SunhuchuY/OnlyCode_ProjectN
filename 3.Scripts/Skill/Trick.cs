using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trick : MonoBehaviour
{
    bool isAleadyUse = false;

    float duration = 3f;

    private void OnEnable()
    {
        isAleadyUse = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Monster>() != null && !isAleadyUse)
        {
            collision.GetComponent<Monster>().trickOn(duration, transform);
            collision.transform.position = transform.position;
            isAleadyUse = true;
        }
    }
}
