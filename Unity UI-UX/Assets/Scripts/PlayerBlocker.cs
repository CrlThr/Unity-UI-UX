using UnityEngine;

public class PlayerBlocker : MonoBehaviour
{
    [Header("References")]
    public Player player;           // Reference to your Player script
    public InventoryUI inventoryUI; // Reference to the inventory UI

    void Update()
    {
        if (inventoryUI == null || player == null) return;

        // Tell Player to block movement if inventory is open
        player.IsBlocked = inventoryUI.IsOpen;
    }
}
