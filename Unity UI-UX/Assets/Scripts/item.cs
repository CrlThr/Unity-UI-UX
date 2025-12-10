using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldableItem : MonoBehaviour
{
    public static HoldableItem leftHand;
    public static HoldableItem rightHand;

    [HideInInspector] public GameObject iconInstance;
    [HideInInspector] public bool isLeftHand;

    public Camera leftHandCamera;
    public Camera rightHandCamera;

    private static RenderTexture slot1Texture;
    private static RenderTexture slot2Texture;

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
        player = playerTransform;

        if (leftHand == null) { leftHand = this; isLeftHand = true; }
        else if (rightHand == null) { rightHand = this; isLeftHand = false; }
        else { Debug.Log("Both hands full"); return; }

        Camera handCam = isLeftHand ? leftHandCamera : rightHandCamera;
        RenderTexture rt = new RenderTexture(256, 256, 16);
        if (isLeftHand) slot1Texture = rt;
        else slot2Texture = rt;

        SetLayerRecursively(transform, LayerMask.NameToLayer("ItemIcon"));

        transform.position = handCam.transform.position + handCam.transform.forward * 2f;
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

        Button btn = iconInstance.AddComponent<Button>();
        btn.onClick.AddListener(Drop);
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
}
