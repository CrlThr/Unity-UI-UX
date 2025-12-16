using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private const int PAGES_PER_VIEW = 2;


    void Start()
    {
        // methode pour naviguer 
        if (nextPageButton != null)
        {
            nextPageButton.onClick.AddListener(NextRecipe);
        }
        if (previousPageButton != null)
        {
            previousPageButton.onClick.AddListener(PreviousRecipe);
        }

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
        if (recipeList == null || recipeList.Count == 0) return;

        // --- 1. Page de Gauche ---
        int leftRecipeIndex = currentPageIndex;

        // la page doit tjr exister si on est en page pairs 
        if (LeftPage != null && leftRecipeIndex < recipeList.Count)
        {
            LeftPage.SetPageContent(recipeList[leftRecipeIndex]);
            LeftPage.gameObject.SetActive(true);
        }
         // gérer les pages impairs
        else if (LeftPage != null)
        {
           
            LeftPage.gameObject.SetActive(false);
        }

        // --- 2. Page de Droite ---
        int rightRecipeIndex = currentPageIndex + 1;

        if (RightPage != null && rightRecipeIndex < recipeList.Count)
        {
            // Si la recette suivante existe, on l'affiche à droite
            RightPage.SetPageContent(recipeList[rightRecipeIndex]);
            RightPage.gameObject.SetActive(true);
        }
        else if (RightPage != null)
        {
            // Si l'index dépasse la liste (cas d'une liste impaire ou fin du livre), 
            // on cache la page de droite.
            RightPage.gameObject.SetActive(false);
        }
        UpdateButtonStates();
    }


    /// <summary>
    /// Gère la navigation vers la page suivante (avance de 2).
    /// </summary>
  
    public void NextRecipe()
    {
        int nextPotentialIndex = currentPageIndex + PAGES_PER_VIEW;

        // Le tour de page est possible si le nouvel index (page de gauche) est dans la liste.
        if (nextPotentialIndex < recipeList.Count)
        {
            currentPageIndex = nextPotentialIndex;
        }
        // Cas spécial pour la dernière page impaire (Ex: Count=7, on passe de index 4 à 6).
        else if (nextPotentialIndex == recipeList.Count && recipeList.Count % 2 != 0)
        {
            // On saute de l'avant-dernière double page (index N-3) à la toute dernière recette (index N-1).
            // Le nouvel index doit être recipeList.Count - 1 (l'index de la dernière recette).
            currentPageIndex = recipeList.Count - 1;
        }
        else
        {
            // Ne rien faire si on est déjà à la fin
            return;
        }

        UpdatePageContent();
    }

    /// <summary>
    /// Gère la navigation vers la page précédente (recule de 2).
    /// </summary>
    public void PreviousRecipe()
    {
        int previousPotentialIndex = currentPageIndex - PAGES_PER_VIEW;

        if (previousPotentialIndex >= 0)
        {
            // Le retour de page est standard
            currentPageIndex = previousPotentialIndex;
        }
        else if (currentPageIndex != 0)
        {
            // Si l'index est 1, on veut revenir à 0 (début du livre)
            currentPageIndex = 0;
        }
        else
        {
            // Ne rien faire si on est déjà au début
            return;
        }

        UpdatePageContent();
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
            // Le bouton 'Suivant' est interactif si :
            // 1. Le prochain tour de page (index + 2) est dans les limites de la liste.
            // 2. OU si c'est le cas de la dernière page impaire (index + 2 mène exactement à la fin de la liste).

            int nextIndex = currentPageIndex + PAGES_PER_VIEW;

            bool canTurn = (nextIndex <= recipeList.Count - 1)
                           || (nextIndex == recipeList.Count && recipeList.Count % 2 != 0);

            nextPageButton.interactable = canTurn;
        }
    }
}