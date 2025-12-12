using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RecipeTitleClicker : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler
{
    private TMP_Text textMeshPro;
    private BookContents bookContents;
    public Texture2D handTexture; // Curseur personnalisé pour le survol

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        bookContents = FindObjectOfType<BookContents>();
        if (bookContents == null)
        {
            Debug.LogError("BookContents script not found! The clicker cannot function.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Utilise la fonction de TextMeshPro pour trouver le lien cliqué
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, eventData.pressEventCamera);

        if (linkIndex > -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            if (linkID.StartsWith("Recipe_"))
            {
                string indexString = linkID.Replace("Recipe_", "");
                if (int.TryParse(indexString, out int recipeIndex))
                {
                    Recipe selectedRecipe = bookContents.GetRecipe(recipeIndex);
                    if (selectedRecipe != null)
                    {
                        bookContents.ShowRecipePopup(selectedRecipe.title, selectedRecipe.fullContent);
                    }
                }
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        // Détecte le survol d'un lien pour changer le curseur (surbrillance visuelle)
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, eventData.pressEventCamera);

        if (linkIndex > -1)
        {
            // Change le curseur en icône de main
            Cursor.SetCursor(handTexture, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            // Réinitialise le curseur
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}