using UnityEngine;
using TMPro;

public class UIObject : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TMP_Text interactionText; 

    private void Start() => HideInteraction();

    public void ShowInteraction(GameObject item)
    {
        if (interactionText != null)
        {
            interactionText.text = $"& Ramasser {item.name} (E)";
            interactionPanel.SetActive(true);
        }
    }

    public void HideInteraction()
    {
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }
}
