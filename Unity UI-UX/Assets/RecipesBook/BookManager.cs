using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    // --- Paramètres Unity ---
    [Header("Recipe settings")]
    [SerializeField] private List<Recipe> recipeList; // La source de données du livre

    [Header("Pages")]
    // Références aux deux composants 'Page' dans la scène
    [SerializeField] private Page LeftPage;
    [SerializeField] private Page RightPage;

    [Header("UI Components")]
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;

    // --- Variables de Gestion ---
    private int currentPageIndex = 0;
    private const int PagesPerView = 2;


    void Start()
    {
        // methode pour naviguer 
        //if (nextPageButton != null)
        //{
        //    if (Mouse.current.leftButton.wasPressedThisFrame)
        //        nextPageButton.onClick.AddListener(NextRecipe);
        //}
        //if (previousPageButton != null)
        //{
        //    if (Mouse.current.leftButton.wasPressedThisFrame)
        //        previousPageButton.onClick.AddListener(PreviousRecipe);
        //}

        // 2. Initialiser et afficher le contenu de départ
        InitializeBookPages();
        UpdatePageContent();
    }

    private void InitializeBookPages()
    {
        // ancien code 
        // Page[] existingPages = pagesContainer.GetComponentsInChildren<Page>(true);
        // LeftPage = existingPages[0];
        // RightPage = existingPages[1];

        // S'assurer que les pages existent et sont actives au départ
        if (LeftPage == null || RightPage == null)
        {
            Debug.LogError("Les références LeftPage et RightPage doivent être assignées dans l'Inspector.");
            return;
        }
        LeftPage.gameObject.SetActive(true);
        RightPage.gameObject.SetActive(true);
    }


    /// <summary>
    /// Met à jour le contenu des deux pages 
    /// </summary>
    private void UpdatePageContent()
    {
        //if (recipeList == null || recipeList.Count == 0)
        //{
        //    // Désactiver les pages si la liste est vide
        //    if (LeftPage != null) LeftPage.gameObject.SetActive(false);
        //    if (RightPage != null) RightPage.gameObject.SetActive(false);
        //    return;
        //}

        // --- 1. Page de Gauche ---
        int leftRecipeIndex = currentPageIndex;

        // doit toujours etre valide et affiché 
        if (LeftPage != null)
        {
            LeftPage.gameObject.SetActive(true);
            LeftPage.SetPageContent(recipeList[leftRecipeIndex]);
        }
     

        // --- 2. Page de Droite ---
        int rightRecipeIndex = currentPageIndex + 1;

        if (RightPage != null && rightRecipeIndex < recipeList.Count)
        {
            // Si la recette suivante existe, on l'affiche à droite
            RightPage.SetPageContent(recipeList[rightRecipeIndex]);
            RightPage.gameObject.SetActive(true);
        }
        else if (RightPage != null) {
            RightPage.SetPageContent(null);
            RightPage.gameObject.SetActive(false);

        }
        UpdateButtonStates();
    }


    /// <summary>
    /// Gère la navigation vers la page suivante (avance de 2).
    /// </summary>
  
    public void NextRecipe()
    {
        int nextIndex = currentPageIndex + PagesPerView; 
        int LastValidIndex = recipeList.Count - 1;  // on check si l'index est valide au départ

        if (nextIndex <= LastValidIndex)
        {
            currentPageIndex = nextIndex;
            UpdatePageContent();
        }
        else
        { 
            // Ne rien faire si on est déjà à la fin
            return;
        }
    }

    /// <summary>
    /// Gère la navigation vers la page précédente (recule de 2).
    /// </summary>
    public void PreviousRecipe()
    {
        int previousPotentialIndex = currentPageIndex - PagesPerView;

        if (previousPotentialIndex >= 0)
        {
            // Le retour de page est standard
            currentPageIndex = previousPotentialIndex;
            UpdatePageContent();
        }
        else if (currentPageIndex != 0)
        {
            // Si l'index est 1, on veut revenir à 0 (début du livre)
            currentPageIndex = 0;
            UpdatePageContent();
        }
        else
        {
            // Ne rien faire si on est déjà au début
            return;
        }
    }


    /// <summary>
    /// Active/Désactive les boutons de navigation.
    /// </summary>
    private void UpdateButtonStates()
    {
        if (recipeList == null || recipeList.Count == 0) return;

        // --- État du bouton 'Précédent' ---
        // Le bouton est interactif si l'index actuel n'est pas 0 (le début).
        if (previousPageButton != null)
        {
            previousPageButton.interactable = (currentPageIndex > 0);
        }

        // --- État du bouton 'Suivant' ---
        if (nextPageButton != null)
        {
            int totalRecipes = recipeList.Count;

            int lastPageIndex = totalRecipes - 1;

            int nextIndex = currentPageIndex + PagesPerView;

            nextPageButton.interactable = (nextIndex <= lastPageIndex);
        }
    }
}