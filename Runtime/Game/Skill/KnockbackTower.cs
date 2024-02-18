using UnityEngine;

public class KnockbackTower : MonoBehaviour
{

    [SerializeField] ParticleSystem particleSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (particleSystem != null)
                particleSystem.Play();
        }
    }
}
