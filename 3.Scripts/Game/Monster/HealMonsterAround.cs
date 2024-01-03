using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMonsterAround : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private List<Monster> healMonsters = new List<Monster>();
    readonly private float effectDuration = 2f;

    public void OnEnable()
    {
        healMonsters.Clear();
    }

    public void healRange(float healAmount)
    {
        for (int i = 0; i < healMonsters.Count; i++)
        {
            healMonsters[i].Health.ApplyModifier((int)healAmount);
        }

        StartCoroutine(healOff());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && collision.gameObject.activeSelf == true)
        {
            healMonsters.Add(collision.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && healMonsters.Contains(collision.GetComponent<Monster>()))
        {
            healMonsters.Remove(collision.GetComponent<Monster>());
        }
    }

    private IEnumerator healOff()
    {
        if (particleSystem == null)
            yield break;

        particleSystem.Play();
        yield return new WaitForSeconds(effectDuration);
        particleSystem.Stop();
    }
}