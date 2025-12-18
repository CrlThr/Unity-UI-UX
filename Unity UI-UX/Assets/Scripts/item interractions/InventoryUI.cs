using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotsCanvas;
    public Transform player;
    public Button closeButton;
    public MonoBehaviour playerController; // drag your movement script here
    public bool IsOpen => slotsCanvas != null && slotsCanvas.activeSelf;

    private void Start()
    {
        slotsCanvas.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    // Opens the fridge UI and locks the player
    public void Open()
    {
        slotsCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = false;
    }

    // Closes the fridge UI and frees the player
    public void Close()
    {
        slotsCanvas.SetActive(false);

        // Restore whatever your game expects; e.g., free mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = true;
    }

}
