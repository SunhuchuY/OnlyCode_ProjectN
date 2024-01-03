using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TimeIntervalFriend : MonoBehaviour
{
    public float coolTime = 0;
    protected float curTime = 0;
    public float duration = 0;

    protected List<Friend> friends = new List<Friend>();

    [SerializeField] 
    protected ParticleSystem particleSystem;

    void OnEnable()
    {
        curTime = coolTime;    
    }


    protected virtual void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > coolTime)
        {

            for (int i = 0; i < friends.Count; i++)
            {
                StartCoroutine(ParticlePause_Delay());
                Action();
            }

            curTime = 0;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Friend>() != null)
        {
            friends.Add(collision.GetComponent<Friend>());
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Friend>() != null && friends.Contains(collision.GetComponent<Friend>()))
        {
            friends.Remove(collision.GetComponent<Friend>());
        }
    }

    protected abstract void Action(); 

    IEnumerator ParticlePause_Delay()
    {
        if (particleSystem == null)
            yield return null;

        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        particleSystem.Stop();
    }
}
