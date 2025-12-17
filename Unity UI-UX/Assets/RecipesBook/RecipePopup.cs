using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipePopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button closeButton; 

    void Start()
    {
        HidePopup();
        closeButton.onClick.AddListener(HidePopup);
    }
    
    public void ShowPopup(Recipe recipe)
    {
        titleText.text = recipe.Name;
        contentText.text = "INGRÉDIENTS :\n" + recipe.Ingredients +
                           "\n\nINSTRUCTIONS :\n" + recipe.Instructions;

        // affichage via le canvas group 
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
   
    public void HidePopup()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false; 
    }
}
