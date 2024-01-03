using UnityEngine;

public class Trick : MonoBehaviour
{
    [SerializeField]
    float duration = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Monster>() != null)
        {
            collision.GetComponent<Monster>().trickOn(duration, transform);
        }
    }
}
