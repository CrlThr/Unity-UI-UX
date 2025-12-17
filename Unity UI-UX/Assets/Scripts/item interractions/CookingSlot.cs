using System.Collections;
using UnityEngine;

public class CookingSlot : MonoBehaviour
{
    public Transform itemPoint;

    private HoldableItem currentItem;
    private Coroutine cookRoutine;

    // Start cooking an item in this slot
    public void StartCooking(HoldableItem item)
    {
        if (item == null) return; // Safety check
        if (currentItem != null) return;

        currentItem = item;
        item.isInSlot = true;

        // Move item to slot
        item.transform.position = itemPoint.position + Vector3.up * 1.5f;
        item.UnfreezePhysics();

        // Only cook if raw
        if (item.CanBeCooked())
            cookRoutine = StartCoroutine(CookProcess(item));
    }

    private IEnumerator CookProcess(HoldableItem item)
    {
        float cookedTimer = 0f;
        float overcookTimer = 0f;

        while (item != null && currentItem == item)
        {
            float multiplier = TemperatureManager.Instance.GetCookingMultiplier();

            if (multiplier > 0f)
            {
                // Cooking raw item
                if (item.cookState == HoldableItem.CookState.Raw)
                {
                    cookedTimer += Time.deltaTime * multiplier;
                    if (cookedTimer >= item.CookTime)
                    {
                        item.cookState = HoldableItem.CookState.Cooked;
                        item.ReplaceWith(item.CuissonItem1);
                    }
                }
                // Overcooking
                else if (item.cookState == HoldableItem.CookState.Cooked)
                {
                    overcookTimer += Time.deltaTime * multiplier;
                    if (overcookTimer >= 5f)
                    {
                        item.cookState = HoldableItem.CookState.Overcooked;
                        item.ReplaceWith(item.CuissonItem2);
                        yield break;
                    }
                }
            }

            yield return null; // wait for next frame
        }
    }

    // Remove item from slot safely
    public void RemoveItem()
    {
        if (cookRoutine != null)
        {
            StopCoroutine(cookRoutine);
            cookRoutine = null;
        }

        if (currentItem != null)
            currentItem.isInSlot = false;

        currentItem = null;
    }

    void OnMouseDown()
    {
        if (currentItem != null)
            RemoveItem();
    }
}
