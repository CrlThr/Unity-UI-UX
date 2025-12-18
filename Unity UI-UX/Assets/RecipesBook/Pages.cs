using TMPro;
using UnityEngine;


/// <summary>
/// Contrôleur de l'affichage d'une seule page de livre. 
/// Il met à jour les composants UI avec les données de la recette fournie.
/// </summary>
public class Page : MonoBehaviour
{
    [Header("Références UI")]
    
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    

    [Header("PopUp")]

    [SerializeField] private RecipeLink titleLink;
    [SerializeField] private RecipePopup popupManager;


    /// <summary>
    /// Met à jour l'affichage de la page avec les données d'une recette.
    /// </summary>
    /// <param name="recipe">La recette à afficher.</param>


    public void SetPageContent(Recipe recipe)
    {
        if (recipe == null)
        {
            titleText.text = "";
            // On vide les autres textes sur le livre
            contentText.text = "";
            if (titleLink != null) titleLink.currentRecipe = null;
        }
        else
        {
            // On affiche que le titre 
            titleText.text = recipe.Name;

            // On laisse les autres textes vides sur le livre
            contentText.text = "";

            if (titleLink != null)
            {
                titleLink.currentRecipe = recipe;
                titleLink.popupSystem = popupManager;
            }
        }
    }
}

