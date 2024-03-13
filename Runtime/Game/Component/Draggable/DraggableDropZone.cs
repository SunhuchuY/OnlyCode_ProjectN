using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class DraggableDropZone : MonoBehaviour, IDropHandler
{
    private const float MULTIPLY_SCALE = 0.35f;

    public event System.Action<GameObject> OnDropped;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        SubscribeAttackRange();
    }

    private void SubscribeAttackRange()
    {
        GameManager.Instance.playerScript.Stats["AttackRange"].OnChangesCurrentValueAsObservable.Subscribe(newValue => 
        {
            float newScale = newValue * MULTIPLY_SCALE;
            transform.localScale = new Vector2(newScale, newScale);
        });
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropped?.Invoke(eventData.pointerDrag);
    }
}