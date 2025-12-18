using UnityEngine;

public class FridgeSlot : MonoBehaviour
{
    public Transform itemPoint;             // where the item visually sits
    public InventoryUI inventoryUI;         // reference to UI to unlock player
    private HoldableItem currentItem;       // stored item in this slot
    public float slotHeightOffset = 0.5f;   // vertical offset

    // Store an item in this slot
    public void StoreItem(HoldableItem item)
    {
        if (item == null || currentItem != null) return;

        currentItem = item;
        item.isInSlot = true;

        // Release from hand
        item.ReleaseHand();

        // Move item to slot
        item.transform.SetParent(null);
        item.transform.position = itemPoint.position + Vector3.up * slotHeightOffset;
        item.transform.rotation = Quaternion.identity;

        // Freeze physics so it stays in place
        item.FreezePhysics();
    }

    // Retrieve the item back to the player
    public void RetrieveItem()
    {
        if (currentItem == null) return;

        currentItem.UnfreezePhysics();
        currentItem.isInSlot = false;

        if (inventoryUI != null && inventoryUI.player != null)
        {
            // Spawn in front of player, like picking it up
            Transform playerTransform = inventoryUI.player;
            Vector3 spawnPos = playerTransform.position + playerTransform.forward * 2f + Vector3.up;
            currentItem.transform.SetParent(null);
            currentItem.transform.position = spawnPos;
            currentItem.transform.rotation = Quaternion.identity;

            currentItem.Pickup(playerTransform, inventoryUI.slotsCanvas.transform); // icon still in UI
        }

        currentItem = null;
    }


    // Called by the Close button on the fridge UI
    public void OnCloseButton()
    {
        if (inventoryUI != null)
            inventoryUI.Close();
    }

    // Click on slot to retrieve the item (like cooking)
    private void OnMouseDown()
    {
        RetrieveItem();
    }
}
