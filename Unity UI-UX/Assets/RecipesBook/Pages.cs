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
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private TextMeshProUGUI instructionsText;


    /// <summary>
    /// Met à jour l'affichage de la page avec les données d'une recette.
    /// </summary>
    /// <param name="recipe">La recette à afficher.</param>
  

    public void SetPageContent(Recipe recipe)
    {
        // Vérification de base pour s'assurer que les références UI sont définies
        if (titleText == null || ingredientsText == null || instructionsText == null)
        {
            Debug.LogError("Les composants Text UI ne sont pas tous liés dans le script Page!", this);
            return;
        }

        if (recipe == null)
        {
            // Si aucune recette n'est fournie (par exemple, dernière page impaire), on vide le contenu
            titleText.text = "Page Vide";
            ingredientsText.text = "";
            instructionsText.text = "";
        }
        else
        {
            // Afficher les données de la recette
            titleText.text = recipe.Name;
            ingredientsText.text = "Ingrédients :\n" + recipe.Ingredients;
            instructionsText.text = "Instructions :\n" + recipe.Instructions;
        }
    }
}

