using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class skill24Assist : MonoBehaviour
{
    // ¥Í¿∏∏È ∆®∞‹≥ø

    [SerializeField] ParticleSystem particleSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            particleSystem.Play();
        }
    }
}
