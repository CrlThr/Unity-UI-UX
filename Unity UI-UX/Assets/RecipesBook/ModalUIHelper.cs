using UnityEngine;
using UnityEngine.UI;

public static class ModalUIHelper
{
    // Règle la visibilité et l'interactivité d'une modale
    public static void OpenModal(CanvasGroup canvasGroup)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Cache la modale
    public static void CloseModal(CanvasGroup canvasGroup)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}