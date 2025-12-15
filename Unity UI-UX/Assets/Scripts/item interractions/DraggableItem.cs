using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;
    public Canvas canvas;

    private Vector2 originalPos;
    private Transform originalParent;

    public HoldableItem linkedItem;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Canvas c, HoldableItem item)
    {
        canvas = c;
        linkedItem = item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        transform.SetParent(canvas.transform, true);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject slot = eventData.pointerEnter;

        if (slot != null && slot.CompareTag("CookingSlot"))
        {
            // Stick item to cooking panel
            transform.SetParent(slot.transform, false);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            // Return to inventory
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalPos;
        }
    }
}
