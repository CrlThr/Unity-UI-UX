using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cook : MonoBehaviour
{
    [SerializeField] List<GameObject> ListCookinghob;
    [SerializeField] Camera mainCam;
    public float interactRange = 10f;

    public GameObject hoverPanel; //pannel faisant comprendre au joueur qu'il peut utiliser le four / à detruire apres la premiere utilisation (le premierclick sur un element de cookinghob
    public TMP_Text hoverText;

    public GameObject cookingPanel; //panel contenant les slots d'ingrediens

    public GameObject ingredient1Panel; //slots ou les preview d'ingredient sont mises apres y avoir drag and drop
    public GameObject ingredient2Panel;
    public GameObject ingredient3Panel;
    public GameObject ingredient4Panel;

    [SerializeField] private CookingSlot[] cookingSlots; 

    private bool isHoveringOven;


    void Update()
    {
        InterractOven();

        if (isHoveringOven && Mouse.current.leftButton.wasPressedThisFrame)
        {
            hoverPanel.SetActive(false);
            cookingPanel.SetActive(true);
        }
    }


    public void InterractOven()
    {
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
        GameObject[] panels =
        {
        ingredient1Panel,
        ingredient2Panel,
        ingredient3Panel,
        ingredient4Panel
    };

        int slotIndex = 0;

        foreach (GameObject panel in panels)
        {
            if (panel.transform.childCount == 0)
                continue;

            HoldableItem item =
                panel.GetComponentInChildren<UIDragItem>()?.linkedItem;

            if (item == null)
                continue;

            if (slotIndex >= cookingSlots.Length)
                break;

            if (!item.CanBeCooked())
                continue;

            // Remove UI icon
            Destroy(item.iconInstance);
            item.isInSlot = false;
            item.iconInstance = null;

            cookingSlots[slotIndex].StartCooking(item);
            slotIndex++;
        }
    }



}