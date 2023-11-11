using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class mon10ScriptAssist : MonoBehaviour
{
    //  positioned component script of heal range gameobject

    [SerializeField] ParticleSystem particleSystem;

    List<Monster> healMonsters = new List<Monster>();

    [SerializeField] int healAmount = 30;
    const float effectDuration = 2f;

    public void OnEnable()
    {
        healMonsters.Clear();
    }

    public void healRange()
    {
        for (int i = 0; i < healMonsters.Count; i++)
        {
            healMonsters[i].currentHealth += healAmount;
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

    IEnumerator healOff()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(effectDuration);
        particleSystem.Stop();
    }
}
