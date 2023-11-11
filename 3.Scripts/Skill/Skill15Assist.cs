using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill15Assist : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (transform.parent.GetComponent<Skill>() != null && other.CompareTag("Monster"))
            {
                other.GetComponent<Monster>().GetDamage(transform.parent.GetComponent<Skill>().attackAmount, true);
                GameManager.particleManager.PlayParticle(other.transform, GameManager.player.transform, 9);
            }
    }

}
