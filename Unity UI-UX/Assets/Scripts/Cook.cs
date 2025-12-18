using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cook : MonoBehaviour
{
    [SerializeField] List<GameObject> ListCookinghob;
    [SerializeField] Camera mainCam;
    public float interactRange = 10f;

    public GameObject hoverPanel;
    public TMP_Text hoverText;

    public GameObject cookingPanel;

    public GameObject ingredient1Panel;
    public GameObject ingredient2Panel;
    public GameObject ingredient3Panel;
    public GameObject ingredient4Panel;

    [SerializeField] private CookingSlot[] cookingSlots;

    private bool isHoveringOven;
    private bool hasUsedOven = false; // prevents hover panel after first use

    private void Start()
    {
        cookingPanel.SetActive(false);
    }

    private void Update()
    {
        InterractOven();

        if (isHoveringOven && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!hasUsedOven)
            {
                hasUsedOven = true; // disable hover panel forever
            }

            hoverPanel.SetActive(false);
            cookingPanel.SetActive(true);
        }
    }

    public void InterractOven()
    {
        if (hasUsedOven)
        {
            hoverPanel.SetActive(false);
            isHoveringOven = false;
            return;
        }

        isHoveringOven = false;
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            foreach (GameObject cookinghob in ListCookinghob)
            {
                if (hit.collider.gameObject == cookinghob)
                {
                    isHoveringOven = true;
                    hoverPanel.SetActive(true);
                    hoverText.text =
                        "Oven. Click to use then drag your inventory in the slots";
                    return;
                }
            }
        }

        hoverPanel.SetActive(false);
    }

    public void StartCooking()
    {
        GameObject[] panels = { ingredient1Panel, ingredient2Panel, ingredient3Panel, ingredient4Panel };
        int slotIndex = 0;

        foreach (GameObject panel in panels)
        {
            if (panel.transform.childCount == 0)
                continue;

            HoldableItem item = panel.GetComponentInChildren<UIDragItem>()?.linkedItem;

            if (item == null || !item.CanBeCooked())
                continue;

            if (slotIndex >= cookingSlots.Length)
                break;

            // Remove UI icon safely
            if (item.iconInstance != null)
            {
                Destroy(item.iconInstance);
                item.iconInstance = null;
            }

            item.isInSlot = false;

            cookingSlots[slotIndex].StartCooking(item);
            slotIndex++;
        }
    }
}