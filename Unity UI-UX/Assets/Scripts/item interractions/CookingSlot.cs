using System.Collections;
using UnityEngine;
using static HoldableItem;

public class CookingSlot : MonoBehaviour
{
    public Transform itemPoint;

    private HoldableItem currentItem;
    private Coroutine cookRoutine;

    public void StartCooking(HoldableItem item)
    {
        if (currentItem != null) return;
        if (!item.CanBeCooked()) return;

        currentItem = item;

        item.isInSlot = false;
        item.transform.position = itemPoint.position;
        item.gameObject.SetActive(true);

        cookRoutine = StartCoroutine(CookProcess(item));
    }

    IEnumerator CookProcess(HoldableItem item)
    {
        yield return new WaitForSeconds(item.CookTime);

        item.cookState = CookState.Cooked;
        item.ReplaceWith(item.CuissonItem1); // <- no assignment

        yield return new WaitForSeconds(5f);

        item.cookState = CookState.Overcooked;
        item.ReplaceWith(item.CuissonItem2); // <- no assignment
    }


    public void RemoveItem()
    {
        if (cookRoutine != null)
        {
            StopCoroutine(cookRoutine);
            cookRoutine = null;
        }

        currentItem = null;
    }

    void OnMouseDown()
    {
        if (currentItem != null)
            RemoveItem();
    }
}
