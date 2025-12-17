using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TextMeshProUGUI textMesh;
    private Color originalColor;
    public Recipe currentRecipe;
    public RecipePopup popupSystem;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color;
    }

    // Surbrillance > enter 
    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = Color.blue;
        textMesh.fontStyle = FontStyles.Underline;
    }

    // > exit
    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = originalColor;
        textMesh.fontStyle = FontStyles.Normal;
    }

    // afficher le poppup

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentRecipe != null && popupSystem != null) 
        { 
            popupSystem.ShowPopup(currentRecipe);
        }

    }
}
