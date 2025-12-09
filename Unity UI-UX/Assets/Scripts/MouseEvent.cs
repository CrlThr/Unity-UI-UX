using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMouseInteract : MonoBehaviour
{
    public Camera mainCam;
    public float interactRange = 10f;
    public Transform uiCanvas;       // Canvas for icons
    public GameObject hoverPanel;
    public TMP_Text hoverText;

    private HoldableItem currentHover;

    void Update()
    {
        DetectHover();

        // Use new Input System for left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame && currentHover != null)
        {
            currentHover.Pickup(transform, uiCanvas);
            hoverPanel.SetActive(false);
            currentHover = null;
        }
    }

    void DetectHover()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            HoldableItem item = hit.collider.GetComponent<HoldableItem>();
            if (item != null)
            {
                if (currentHover != item)
                {
                    currentHover = item;
                    hoverPanel.SetActive(true);
                    hoverText.text = "Ramasser " + item.name;
                }
                return;
            }
        }

        // No interactable object detected
        currentHover = null;
        hoverPanel.SetActive(false);
    }
}
