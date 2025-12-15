using TMPro;
using UnityEngine;
using System.Collections.Generic; 
using UnityEngine.UI; 

// 1. Structure de données (doit être System.Serializable pour apparaître dans l'Inspector)
[System.Serializable]
public class Recipe
{
    public string title = "Nouvelle Recette";
    [TextArea(5, 10)]
    public string fullContent = "Contenu détaillé de la recette.";
}


public class BookContents : MonoBehaviour
{
    [Header("Book Data")]
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    private string generatedContent = "";
   
    [Space]
    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space]
    [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;

   
    [Header("Recette Popup (Lecture)")]
    [SerializeField] private CanvasGroup recipePopup;
    [SerializeField] private TMP_Text recipeTitleText;
    [SerializeField] private TMP_Text recipeContentText;

    [Header("Ajout Recette Popup")]
    [SerializeField] private CanvasGroup addRecipePopup;
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField contentInput;
   
    private void Awake()
    {
        SetupContent();
        UpdatePagination();
        HideRecipePopup();
        HideAddRecipePopup();
    }

   
    // 3. Gestion du Contenu et Pagination

    // Génère le texte des titres avec les balises <link>
    private void SetupContent()
    {
        generatedContent = "";
        for (int i = 0; i < recipes.Count; i++)
        {
            
            // La balise <link> est essentielle pour que le script de clic fonctionne.
            generatedContent += $"<link=\"Recipe_{i}\">**{recipes[i].title}**</link>\n";
        }

        leftSide.text = generatedContent;
        rightSide.text = generatedContent;

        // Force la mise à jour pour que TextMeshPro recalcule les pages
        leftSide.ForceMeshUpdate();
        rightSide.ForceMeshUpdate();

        // on s'assure d'afficher la première page
        leftSide.pageToDisplay = 1;
        rightSide.pageToDisplay = (leftSide.textInfo.pageCount > 1) ? 2 : 1;
        UpdatePagination();
    }

    // Méthode publique pour récupérer les données d'une recette par son index
    public Recipe GetRecipe(int index)
    {
        if (index >= 0 && index < recipes.Count)
        {
            return recipes[index];
        }
        return null;
    }

    // Mise à jour de la pagination 
    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
    }

    // Page Précédente 
    public void PreviousPage()
    {
        if (leftSide.pageToDisplay <= 1)
        {
            leftSide.pageToDisplay = 1;
            rightSide.pageToDisplay = 2; // S'assurer que les deux pages sont visibles
            UpdatePagination();
            return;
        }

        // Avance de deux pages en arrière
        leftSide.pageToDisplay -= 2;
        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;

        UpdatePagination();
    }

   
    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;

        // Si on n'est pas à la dernière page, avance de deux pages
        if (leftSide.pageToDisplay < leftSide.textInfo.pageCount - 1)
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else // Sinon, met la dernière double page visible
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.textInfo.pageCount;
        }

        UpdatePagination();
    }
    
    // 4. Logique du Popup de Lecture

    public void ShowRecipePopup(string title, string content)
    {
        recipeTitleText.text = title;
        recipeContentText.text = content;

        recipePopup.alpha = 1f;
        recipePopup.blocksRaycasts = true;
        recipePopup.interactable = true;
    }

    public void HideRecipePopup()
    {
        recipePopup.alpha = 0f;
        recipePopup.blocksRaycasts = false;
        recipePopup.interactable = false;
    }
   
    // 5. Logique du Popup d'Ajout

    
    public void ShowAddRecipePopup()
    {
        titleInput.text = "";
        contentInput.text = "";

        addRecipePopup.alpha = 1f;
        addRecipePopup.blocksRaycasts = true;
        addRecipePopup.interactable = true;
    }

    
    public void HideAddRecipePopup()
    {
        addRecipePopup.alpha = 0f;
        addRecipePopup.blocksRaycasts = false;
        addRecipePopup.interactable = false;
    }

    
    public void AddRecipeFromInput()
    {
        string newTitle = titleInput.text.Trim();
        string newContent = contentInput.text;

        if (!string.IsNullOrWhiteSpace(newTitle) && !string.IsNullOrWhiteSpace(newContent))
        {
            Recipe newRecipe = new Recipe
            {
                title = newTitle,
                fullContent = newContent
            };

            recipes.Add(newRecipe);
            SetupContent(); // Régénère la liste des titres pour inclure le nouveau
            HideAddRecipePopup();

            // On s'assure d'aller à la nouvelle page si elle a été créée
            NextPage();
        }
        else
        {
            Debug.LogWarning("Le titre et le contenu ne doivent pas être vides.");
        }
    }
    
}