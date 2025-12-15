using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/New Recipe", order = 1)]
public class RecipeSO : ScriptableObject
{
    // Recipe properties
    public string title = "Recipe Name";

    [TextArea(5, 10)]
    public string description = "Detailed preparation steps...";

    // Ingredients list
    public List<string> ingredients = new List<string>()
    {
        "200g of Flour",
        "3 Eggs",
        "100ml of Milk"
    };

    /// <summary>
    /// Returns the ingredients content formatted with bullet points.
    /// </summary>
    public string GetFormattedIngredients()
    {
        string content = "";
        foreach (string ingredient in ingredients)
        {
            content += $"- {ingredient}\n";
        }
        return content;
    }
}

