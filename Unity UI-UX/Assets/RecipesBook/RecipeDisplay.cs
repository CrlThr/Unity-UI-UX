using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RecipeDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private BookManager manager;
    private RecipeSO associatedRecipe;
    private TextMeshProUGUI textComponent;
    private Color normalColor = Color.black;
    private Color highlightColor = Color.blue;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void SetRecipe(BookManager bookManager, RecipeSO recipe)
    {
        manager = bookManager;
        associatedRecipe = recipe;
        textComponent.text = recipe.title;
        textComponent.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textComponent != null) textComponent.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textComponent != null) textComponent.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Now calls the integrated function on the BookManager
        manager.OpenRecipeReader(associatedRecipe);
    }
}