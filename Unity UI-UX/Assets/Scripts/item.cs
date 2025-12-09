using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldableItem : MonoBehaviour
{
    public static HoldableItem leftHand;
    public static HoldableItem rightHand;

    [HideInInspector] public GameObject iconInstance;
    [HideInInspector] public bool isLeftHand;

    private static Camera iconCamera;
    private static RenderTexture slot1Texture;
    private static RenderTexture slot2Texture;

    private Transform player;

    void SetupIconCamera()
    {
        if (iconCamera != null) return;

        GameObject camGO = new GameObject("HiddenIconCamera");
        iconCamera = camGO.AddComponent<Camera>();
        iconCamera.clearFlags = CameraClearFlags.Color;
        iconCamera.backgroundColor = Color.clear;
        iconCamera.cullingMask = LayerMask.GetMask("ItemIcon");
        iconCamera.enabled = false;
    }

    // Call when picking up the item
    public void Pickup(Transform playerTransform, Transform canvas)
    {
        player = playerTransform;

        // Pick a free slot
        if (leftHand == null) { leftHand = this; isLeftHand = true; }
        else if (rightHand == null) { rightHand = this; isLeftHand = false; }
        else { Debug.Log("Both hands full"); return; }

        SetupIconCamera();

        // Hide the object in world
        gameObject.SetActive(false);

        // Create RenderTexture for this item
        RenderTexture rt = new RenderTexture(256, 256, 16);
        if (isLeftHand) slot1Texture = rt; else slot2Texture = rt;

        // Move item in front of the icon camera
        gameObject.SetActive(true); // temporarily visible for rendering
        gameObject.layer = LayerMask.NameToLayer("ItemIcon");
        transform.SetParent(iconCamera.transform, false);
        transform.localPosition = Vector3.forward * 2f;
        transform.localRotation = Quaternion.identity;

        // Render to texture
        iconCamera.targetTexture = rt;
        iconCamera.Render();

        // Create UI icon
        iconInstance = new GameObject("ItemIcon");
        iconInstance.transform.SetParent(canvas, false);

        RawImage raw = iconInstance.AddComponent<RawImage>();
        raw.texture = rt;

        RectTransform rtUI = iconInstance.GetComponent<RectTransform>();
        rtUI.sizeDelta = new Vector2(64, 64);
        rtUI.anchorMin = rtUI.anchorMax = isLeftHand ? new Vector2(0.1f, 0.9f) : new Vector2(0.9f, 0.9f);
        rtUI.anchoredPosition = Vector2.zero;

        Button btn = iconInstance.AddComponent<Button>();
        btn.onClick.AddListener(Drop);
    }

    public void Drop()
    {
        if (isLeftHand) { leftHand = null; slot1Texture = null; }
        else { rightHand = null; slot2Texture = null; }

        // Remove UI icon
        if (iconInstance != null) Destroy(iconInstance);

        // Restore object in world
        gameObject.SetActive(true);
        transform.SetParent(null);
        transform.position = player.position + player.forward * 1.5f;
        gameObject.layer = 0; // default layer
    }
}
