using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill25Assist : MonoBehaviour
{
    // Heal Range Script 
    const float coolTime = 2f;
    float curTime = 0f;

    float speedUpAmount = 0.7f;

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
                StartCoroutine(SpeedUp_Coroutine(friends[i] , speedUpAmount));
            }

            curTime = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Friend>() != null)
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

    IEnumerator SpeedUp_Coroutine(Friend friend ,float changeToAddSpeed)
    {
        float originalSpeed = friend.moveSpeed;
        friend.moveSpeed += changeToAddSpeed;

        yield return new WaitForSeconds(1f);

        friend.moveSpeed = originalSpeed;

    }
}
