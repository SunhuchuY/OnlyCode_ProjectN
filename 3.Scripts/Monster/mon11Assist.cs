using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mon11Assist : MonoBehaviour
{
    [SerializeField] private Monster monster;

    private List<GameObject> FriendAndPlayer = new List<GameObject>();

    private void OnDisable()
    {
        for (int i = 0; i < FriendAndPlayer.Count; i++)
        {
            if (FriendAndPlayer[i].GetComponent<Player>() != null)
                FriendAndPlayer[i].GetComponent<Player>().GetDamage(monster.attackMount);
            else if (FriendAndPlayer[i].GetComponent<Friend>() != null)
                FriendAndPlayer[i].GetComponent<Friend>().GetDamage(monster.attackMount);
        }


        StopCoroutine(SuicideDelay_Corutine());
        GameManager.Instance.particleManager.PlayParticle(transform, transform, 17);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !FriendAndPlayer.Contains(collision.gameObject))
        {
            FriendAndPlayer.Add(collision.gameObject);
            StartCoroutine(SuicideDelay_Corutine());
        }
    }

    IEnumerator SuicideDelay_Corutine()
    {
        yield return new WaitForSeconds(0.5f);
        monster.Health.ApplyModifier(-monster.Health.Cap);
    }
}