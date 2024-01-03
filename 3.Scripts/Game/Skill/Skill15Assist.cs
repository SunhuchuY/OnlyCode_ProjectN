using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill15Assist : MonoBehaviour
{ // Heal Range Script 
    const float coolTime = 2f;
    float curTime = 0f;

    int healAmount = 10;

    List<Friend> friends = new List<Friend>();
    [SerializeField] ParticleSystem particleSystem;

    private void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > coolTime)
        {
            StartCoroutine(ParticlePause_Delay());

            for (int i = 0; i < friends.Count; i++)
            {
                friends[i].currentHealth += healAmount;
            }

            curTime = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Friend>() != null)
        {
            friends.Add(collision.GetComponent<Friend>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Friend>() != null && friends.Contains(collision.GetComponent<Friend>()))
        {
            friends.Remove(collision.GetComponent<Friend>());
        }
    }

    IEnumerator ParticlePause_Delay()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        particleSystem.Stop();
    }
}
