using UnityEngine;

public class skill28Assist : MonoBehaviour
{

    int count = 0;


    [SerializeField] Skill skill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            //collision.GetComponent<Monster>().GetDamage(skill.GetattackAmount(), true);
        }
    }
}
