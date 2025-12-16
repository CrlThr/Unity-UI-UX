using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BookManager : MonoBehaviour
{
    // --- Data ---
    public List<RecipeSO> allRecipes;
    public int recipesPerPage = 4;
    private int currentPageStartIndex = 0;

    // --- References UI - Book Pages ---
    public GameObject recipeTitlePrefab;
    public Transform leftPagePanel;
    public Transform rightPagePanel;
    public GameObject leftArrow;
    public GameObject rightArrow;

    // --- References UI - Modal Reader (Fusionné) ---
    [Header("Modal Reader References")]
    public CanvasGroup readerCanvasGroup;
    public TextMeshProUGUI readerTitleText;
    public TextMeshProUGUI readerIngredientsText;
    public TextMeshProUGUI readerDescriptionText;

    // --- References UI - Modal Adder (Fusionné) ---
    [Header("Modal Adder References")]
    public CanvasGroup adderCanvasGroup;
    public TMP_InputField adderTitleInput;
    public TMP_InputField adderDescriptionInput;
    public TMP_Dropdown adderIngredientsDropdown;
    public Transform adderIngredientsListParent;
    public GameObject ingredientDisplayPrefab;
    public List<string> possibleIngredients = new List<string> { "Flour", "Eggs", "Milk", "Butter", "Sugar" };

    private List<string> addedIngredients = new List<string>();


    void Start()
    {
        // Initial setup for Modal Adder Dropdown
        adderIngredientsDropdown.ClearOptions();
        adderIngredientsDropdown.AddOptions(possibleIngredients);

        DisplayPage(0);
        CloseAllModals();
    }


    public void DisplayPage(int startIndex)
    {
        ClearPage(leftPagePanel);
        ClearPage(rightPagePanel);

        currentPageStartIndex = startIndex;
        int currentRecipeIndex = startIndex;

        // Display Left Page
        for (int i = 0; i < recipesPerPage && currentRecipeIndex < allRecipes.Count; i++)
        {
            InstantiateRecipeTitle(allRecipes[currentRecipeIndex], leftPagePanel);
            currentRecipeIndex++;
        }

        // Display Right Page
        for (int i = 0; i < recipesPerPage && currentRecipeIndex < allRecipes.Count; i++)
        {
            InstantiateRecipeTitle(allRecipes[currentRecipeIndex], rightPagePanel);
            currentRecipeIndex++;
        }

        UpdateArrows();
    }

    private void InstantiateRecipeTitle(RecipeSO recipe, Transform parent)
    {
        GameObject titleObj = Instantiate(recipeTitlePrefab, parent);
        RecipeDisplay display = titleObj.GetComponent<RecipeDisplay>();
        if (display != null)
        {
            // Set the manager reference to THIS script (BookManager)
            display.SetRecipe(this, recipe);
        }
    }

    private void ClearPage(Transform parent)
    {
        while (parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    public void NextPage()
    {
        int newStart = currentPageStartIndex + (recipesPerPage * 2);
        if (newStart < allRecipes.Count) DisplayPage(newStart);
    }

    public void PreviousPage()
    {
        int newStart = currentPageStartIndex - (recipesPerPage * 2);
        if (newStart >= 0) DisplayPage(newStart);
    }

    private void UpdateArrows()
    {
        leftArrow.SetActive(currentPageStartIndex > 0);
        rightArrow.SetActive(currentPageStartIndex + (recipesPerPage * 2) < allRecipes.Count);
    }

    public void CloseAllModals()
    {
        ModalUIHelper.CloseModal(readerCanvasGroup);
        ModalUIHelper.CloseModal(adderCanvasGroup);
    }

 
    public void OpenRecipeReader(RecipeSO recipe)
    {
        CloseAllModals(); // Close Adder if it was open

        // 1. Load content
        readerTitleText.text = recipe.title;
        readerIngredientsText.text = recipe.GetFormattedIngredients();
        readerDescriptionText.text = recipe.description;

        // 2. Open Modal
        ModalUIHelper.OpenModal(readerCanvasGroup);
    }

    public void OpenAddRecipeModal()
    {
        Debug.Log($"Canvas Group status: {adderCanvasGroup != null}");
        CloseAllModals(); // Close Reader if it was open

        // Cleanup and reset state
        adderTitleInput.text = "";
        adderDescriptionInput.text = "";
        addedIngredients.Clear();
        ClearIngredientList(adderIngredientsListParent);

        ModalUIHelper.OpenModal(adderCanvasGroup);
    }

    public void AddIngredientFromDropdown()
    {
        string ingredient = adderIngredientsDropdown.options[adderIngredientsDropdown.value].text;

        if (!addedIngredients.Contains(ingredient))
        {
            addedIngredients.Add(ingredient);
            DisplayIngredientInList(ingredient);
        }
    }

    private void DisplayIngredientInList(string ingredient)
    {
        GameObject ingredientObj = Instantiate(ingredientDisplayPrefab, adderIngredientsListParent);
        TextMeshProUGUI tmp = ingredientObj.GetComponent<TextMeshProUGUI>();
        if (tmp != null) tmp.text = $"- {ingredient}";
    }

    private void ClearIngredientList(Transform parent)
    {
        while (parent.childCount > 0) Destroy(parent.GetChild(0).gameObject);
    }

    public void SaveNewRecipe()
    {
        if (string.IsNullOrEmpty(adderTitleInput.text)) return;

        RecipeSO newRecipe = ScriptableObject.CreateInstance<RecipeSO>();
        newRecipe.title = adderTitleInput.text;
        newRecipe.ingredients = addedIngredients.ToList();
        newRecipe.description = adderDescriptionInput.text;

        allRecipes.Add(newRecipe);

        // Refresh display to show the new recipe
        DisplayPage(currentPageStartIndex);

        CloseAllModals();
    }
}