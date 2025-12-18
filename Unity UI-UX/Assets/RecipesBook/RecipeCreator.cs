using UnityEngine;
using TMPro;

public class RecipeCreator : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_InputField contentInput;
    public CanvasGroup canvasGroup;

    [Header("Data Reference")]
    public BookManager bookManager;

    private void Start()
    {
        // On s'assure juste que le panel est bien caché au début
        CloseCreator();
    }

    public void OpenCreator()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        // On s'assure que le panel est devant les autres
        transform.SetAsLastSibling();
    }

    public void SaveNewRecipe()
    {
        // Sécurité : ne pas enregistrer si le nom est vide
        if (string.IsNullOrWhiteSpace(nameInput.text)) return;

        Recipe newRecipe = new Recipe();
        newRecipe.Name = nameInput.text;
        newRecipe.Content = contentInput.text;

        bookManager.recipeList.Add(newRecipe);
        bookManager.UpdatePageContent();
        CloseCreator();
    }

    public void CloseCreator()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        nameInput.text = "";
        contentInput.text = "";
    }
}