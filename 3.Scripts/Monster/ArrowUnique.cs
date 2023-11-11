using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUnique : MonoBehaviour
{
    const int instantiateCount = 10;

    [SerializeField] GameObject bounceArrowPrefeb;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < instantiateCount; i++)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, transform.rotation.z + (i * 30));
                GameObject obj = Instantiate(bounceArrowPrefeb, transform.position, rotation);
            }
        }
    }
}
