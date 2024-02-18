using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event System.Action OnDragStart;
    public event System.Action OnDragEnd;
    public event System.Action<DraggableDropZone, Vector3> OnDropInZone;
    public event System.Action OnNotDropInZone; 

    private const float DRAG_THRESHOLD = 1f; // 드래그를 시작하기 위한 임계값입니다.

    private Vector3 originPosition;
    private int siblingIndexOrigin;
    private LayoutElement layoutElement;
    private CanvasGroup canvasGroup;
    private HorizontalOrVerticalLayoutGroup layoutGroup;

    private void Start()
    {
        layoutElement = GetComponent<LayoutElement>();
        canvasGroup = GetComponent<CanvasGroup>();
        layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originPosition = eventData.position;
        if (layoutElement != null)
            layoutElement.ignoreLayout = true;
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        OnDragStart?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 _mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        if (Vector3.Distance(originPosition, _mouseWorldPosition) > DRAG_THRESHOLD)
            transform.position = _mouseWorldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 _mouseWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int _layerMask = 1 << LayerMask.NameToLayer("DropZone");
        RaycastHit2D _hit = Physics2D.GetRayIntersection(_ray, 1000f, _layerMask);

        if (layoutElement != null)
            layoutElement.ignoreLayout = false;
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        // 원래 위치로 되돌립니다.
        // 만약 드래그하여 위치가 변경되어야 하는 경우, OnDragEnd 또는 이 Draggable이 놓인 DropZone에서 위치를 변경해주어야 합니다.
        transform.position = originPosition;

        if (Vector3.Distance(originPosition, _mouseWorldPosition) > DRAG_THRESHOLD)
            OnDragEnd?.Invoke();

        if (_hit.collider != null)
        {
            var _dropZone = _hit.collider.GetComponent<DraggableDropZone>();

            OnDropInZone?.Invoke(_dropZone, _mouseWorldPosition);
            _dropZone.OnDrop(eventData);
        }
        else
        {
            OnNotDropInZone?.Invoke();
        }
    }
}