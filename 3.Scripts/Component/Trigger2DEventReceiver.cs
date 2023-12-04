using UnityEngine;

public class Trigger2DEventReceiver : MonoBehaviour
{
    public event System.Action<Collider2D> OnTriggerEnter2DEvent;
    public event System.Action<Collider2D> OnTriggerStay2DEvent;
    public event System.Action<Collider2D> OnTriggerExit2DEvent;

    private void OnTriggerEnter2D(Collider2D collision)
        => OnTriggerEnter2DEvent?.Invoke(collision);

    private void OnTriggerStay2D(Collider2D collision)
        => OnTriggerStay2DEvent?.Invoke(collision);

    private void OnTriggerExit2D(Collider2D collision)
        => OnTriggerExit2DEvent?.Invoke(collision);
}