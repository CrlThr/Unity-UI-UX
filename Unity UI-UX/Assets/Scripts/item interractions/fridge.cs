using UnityEngine;
using UnityEngine.InputSystem;

public class Fridge : MonoBehaviour
{
    [Header("References")]
    public Camera mainCam;
    public InventoryUI inventoryUI;

    [Header("Hover UI")]
    public GameObject hoverPanel;

    public float interactRange = 4f;

    private bool isHovering;

    void Start()
    {
        if (hoverPanel != null)
            hoverPanel.SetActive(false);
    }

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        isHovering = false;

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isHovering = true;
                hoverPanel.SetActive(true);
                return;
            }
        }

        hoverPanel.SetActive(false);
    }

    void HandleClick()
    {
        if (!isHovering) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        hoverPanel.SetActive(false);
        inventoryUI.Open();
    }
}
