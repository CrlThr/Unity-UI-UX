using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldableItem : MonoBehaviour
{

    public enum CookState
    {
        Raw,
        Cooked,
        Overcooked
    }

    public static HoldableItem leftHand;
    public static HoldableItem rightHand;

    [HideInInspector] public GameObject iconInstance;
    [HideInInspector] public bool isLeftHand;

    public Camera leftHandCamera;
    public Camera rightHandCamera;

    public bool isInSlot = false;
    private static RenderTexture slot1Texture;
    private static RenderTexture slot2Texture;

    public CookState cookState = CookState.Raw;

    [SerializeField] GameObject cuissonItem1;
    [SerializeField] GameObject cuissonItem2;

    [SerializeField] float cookTime;

    public float CookTime => cookTime;
    public GameObject CuissonItem1 => cuissonItem1;
    public GameObject CuissonItem2 => cuissonItem2;



    private Transform player;

    // Utility function to set layer for object and all children
    private void SetLayerRecursively(Transform obj, int layer)
    {
        obj.gameObject.layer = layer;
        foreach (Transform child in obj)
            SetLayerRecursively(child, layer);
    }

    public void Pickup(Transform playerTransform, Transform canvas)
    {
        if (iconInstance != null)
            Destroy(iconInstance);

        player = playerTransform;

        if (leftHand == null) { leftHand = this; isLeftHand = true; }
        else if (rightHand == null) { rightHand = this; isLeftHand = false; }
        else { Debug.Log("Both hands full"); return; }

        Camera handCam = isLeftHand ? leftHandCamera : rightHandCamera;
        RenderTexture rt = new RenderTexture(256, 256, 16);
        if (isLeftHand) slot1Texture = rt;
        else slot2Texture = rt;

        SetLayerRecursively(transform, LayerMask.NameToLayer("ItemIcon"));

        transform.position = handCam.transform.position + handCam.transform.forward * 3f;
        transform.rotation = Quaternion.identity;

        handCam.enabled = true;
        handCam.targetTexture = rt;
        handCam.Render();
        handCam.enabled = false; // deactivate immediately after rendering

        iconInstance = new GameObject("ItemIcon");
        iconInstance.transform.SetParent(canvas, false);

        RawImage raw = iconInstance.AddComponent<RawImage>();
        raw.texture = rt;

        RectTransform rtUI = iconInstance.GetComponent<RectTransform>();
        rtUI.sizeDelta = new Vector2(64, 64);
        rtUI.anchorMin = rtUI.anchorMax = isLeftHand ? new Vector2(0.1f, 0.1f) : new Vector2(0.9f, 0.1f);
        rtUI.anchoredPosition = Vector2.zero;

        CanvasGroup cg = iconInstance.AddComponent<CanvasGroup>();

        UIDragItem drag = iconInstance.AddComponent<UIDragItem>();
        drag.Init(canvas.GetComponent<Canvas>(), this);

        Button btn = iconInstance.AddComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            if (isInSlot)
                PickupFromSlot(canvas, player);
            else
                Drop();
        });

    }

    public bool CanBeCooked()
    {
        return cookState == CookState.Raw;
    }


    public void ReplaceWith(GameObject newPrefab)
    {
        // Destroy old visuals
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // Instantiate new visuals as child
        GameObject newVisual = Instantiate(newPrefab, transform);
        newVisual.transform.localPosition = Vector3.zero;
        newVisual.transform.localRotation = Quaternion.identity;

    }

    public void Drop()
    {
        // Clear hand assignment and camera texture
        if (isLeftHand)
        {
            leftHand = null;
            leftHandCamera.targetTexture = null;
        }
        else
        {
            rightHand = null;
            rightHandCamera.targetTexture = null;
        }

        // Remove UI icon
        if (iconInstance != null) Destroy(iconInstance);

        // Place object back in the world
        gameObject.SetActive(true);
        transform.SetParent(null);
        transform.position = player.position + player.forward * 3f + Vector3.up ;
        gameObject.layer = 0; // reset layer to default
    }

    public void ReleaseHand()
    {
        if (isLeftHand && leftHand == this)
        {
            leftHand = null;
            leftHandCamera.targetTexture = null;
        }
        else if (!isLeftHand && rightHand == this)
        {
            rightHand = null;
            rightHandCamera.targetTexture = null;
        }
    }

    public void PickupFromSlot(Transform canvas, Transform playerTransform)
    {
        if (leftHand != null && rightHand != null)
        {
            Debug.Log("No free hand");
            return;
        }

        // Supprimer l'icône du slot
        if (iconInstance != null)
            Destroy(iconInstance);

        isInSlot = false;

        // Reprendre normalement
        Pickup(playerTransform, canvas);
    }


}
