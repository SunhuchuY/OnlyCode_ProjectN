using UnityEngine;

public class Skill15Assist_Remove : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (transform.parent.GetComponent<Skill>() != null && other.CompareTag("Monster"))
            {
                //other.GetComponent<Monster>().GetDamage(transform.parent.GetComponent<Skill>().GetattackAmount(), true);
                GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/10", other.transform.position);
            }
    }

}
