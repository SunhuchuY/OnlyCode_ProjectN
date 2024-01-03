using System.Collections.Generic;
using UnityEngine;

public class RangeTower : MonoBehaviour
{

    public List<Monster> monsters { get; private set; } = new List<Monster>();

    void OnEnable()
    {
        monsters.Clear();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;

        if (collision.GetComponent<Monster>() == null)
            return;

        var monster = collision.GetComponent<Monster>();

        if (monsters.Contains(monster))
            return;

        monsters.Add(monster);
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;

        if (collision.GetComponent<Monster>() == null)
            return;

        var monster = collision.GetComponent<Monster>();

        if (!monsters.Contains(monster))
            return;

        monsters.Remove(monster);
    }

}
