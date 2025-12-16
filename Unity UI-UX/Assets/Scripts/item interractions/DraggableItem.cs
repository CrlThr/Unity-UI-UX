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
            // Libérer la main
            linkedItem.ReleaseHand();
            linkedItem.isInSlot = true;

            // Attacher au slot
            transform.SetParent(slot.transform, false);

            // Remplir tout le panel
            RectTransform slotRT = slot.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            // Retour inventaire
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalPos;
        }
    }

}
