using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableDropZone : MonoBehaviour, IDropHandler
{
    public event System.Action<GameObject> OnDropped;

    public void OnDrop(PointerEventData eventData)
    {
        OnDropped?.Invoke(eventData.pointerDrag);
    }
}