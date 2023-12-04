using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill15Assist_Remove : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (transform.parent.GetComponent<Skill>() != null && other.CompareTag("Monster"))
            {
                other.GetComponent<Monster>().GetDamage(transform.parent.GetComponent<Skill>().attackAmount, true);
                GameManager.Instance.particleManager.PlayParticle(other.transform, GameManager.Instance.player.transform, 9);
            }
    }

}
