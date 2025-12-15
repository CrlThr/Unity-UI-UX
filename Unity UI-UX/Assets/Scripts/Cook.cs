using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cook : MonoBehaviour
{
    [SerializeField] GameObject cookinghob;
    [SerializeField] GameObject Aliment;
    [SerializeField] Camera mainCam;
    public float interactRange = 10f;

    public GameObject hoverPanel; 
    public TMP_Text hoverText;

    public GameObject cookingPanel;
    public TMP_Text cookingText;

    public GameObject ingredient1Panel;
    public GameObject ingredient2Panel;
    public GameObject ingredient3Panel;
    public GameObject ingredient4Panel;

    void Update()
    {

        InterractOven();

        if (Mouse.current.leftButton.wasPressedThisFrame && cookinghob != null)
        {
            hoverPanel.SetActive(false);
            cookingPanel.SetActive(true);
            //in the cookingPanel there will be the four ingredientpanel 
            //if we drag and drop the item from our inventory to one panel the item stay stuck here if not it will come back to us when we release the click
        }
    }

    public void InterractOven()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.GetComponent<GameObject>() != null)
            {
                if (hit.collider.GetComponent<GameObject>() == cookinghob)
                {
                    hoverPanel.SetActive(true);
                    hoverText.text = "Oven. click to use";
                }
                return;
            }
        }

        hoverPanel.SetActive(false);
    }

}