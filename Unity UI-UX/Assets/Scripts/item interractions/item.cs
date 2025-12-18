using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldableItem : MonoBehaviour
{
    private GameObject currentVisual;

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

    private Rigidbody rb;
    private bool frozenBySlot;

    public CookState cookState = CookState.Raw;

    [SerializeField] GameObject cuissonItem1;
    [SerializeField] GameObject cuissonItem2;
    [SerializeField] float cookTime;

    public float CookTime => cookTime;
    public GameObject CuissonItem1 => cuissonItem1;
    public GameObject CuissonItem2 => cuissonItem2;

    private Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 🔴 CRITICAL FIX:
        // Register the RAW mesh already present in the scene
        if (currentVisual == null)
        {
            Renderer r = GetComponentInChildren<Renderer>();
            if (r != null)
            {
                currentVisual = r.gameObject;
            }
            else
            {
                Debug.LogError($"[{name}] No Renderer found for RAW visual!", this);
            }
        }
    }

    #region PICKUP / DROP

    public void Pickup(Transform playerTransform, Transform canvas)
    {
        if (iconInstance != null)
            Destroy(iconInstance);

        UnfreezePhysics();
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
        handCam.enabled = false;

        iconInstance = new GameObject("ItemIcon");
        iconInstance.transform.SetParent(canvas, false);

        RawImage raw = iconInstance.AddComponent<RawImage>();
        raw.texture = rt;

        RectTransform rtUI = iconInstance.GetComponent<RectTransform>();
        rtUI.sizeDelta = new Vector2(64, 64);
        rtUI.anchorMin = rtUI.anchorMax =
            isLeftHand ? new Vector2(0.1f, 0.1f) : new Vector2(0.9f, 0.1f);
        rtUI.anchoredPosition = Vector2.zero;

        iconInstance.AddComponent<CanvasGroup>();

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

    public void Drop()
    {
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

        if (iconInstance != null)
            Destroy(iconInstance);

        isInSlot = false;

        transform.SetParent(null);
        transform.position = player.position + player.forward * 3f + Vector3.up;
        gameObject.layer = 0;

        rb.constraints = RigidbodyConstraints.None;
    }

    public void PickupFromSlot(Transform canvas, Transform playerTransform)
    {
        if (leftHand != null && rightHand != null)
            return;

        UnfreezePhysics();

        if (iconInstance != null)
            Destroy(iconInstance);

        isInSlot = false;
        Pickup(playerTransform, canvas);
    }

    #endregion

    #region COOKING

    public bool CanBeCooked()
    {
        return cookState == CookState.Raw;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isInSlot) return;

        if (collision.gameObject.CompareTag("CookingSurface"))
        {
            FreezePhysics();
        }
    }

    #endregion

    #region REPLACE VISUAL

    public void ReplaceWith(GameObject newPrefab)
    {
        if (newPrefab == null || currentVisual == null) return;

        MeshFilter newMF = newPrefab.GetComponent<MeshFilter>();
        MeshRenderer newMR = newPrefab.GetComponent<MeshRenderer>();

        MeshFilter currentMF = currentVisual.GetComponent<MeshFilter>();
        MeshRenderer currentMR = currentVisual.GetComponent<MeshRenderer>();

        if (currentMF != null && newMF != null)
            currentMF.mesh = newMF.sharedMesh;

        if (currentMR != null && newMR != null)
            currentMR.materials = newMR.sharedMaterials;

        // Keep the visual relative to the root
        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;
        currentVisual.transform.localScale = Vector3.one;

        // Calculate vertical offset based on mesh bounds (local space)
        Bounds rootBounds = GetCombinedBounds(gameObject); // root HoldableItem
        Bounds visualBounds = GetCombinedBounds(currentVisual);
        float yOffset = rootBounds.min.y - visualBounds.min.y;
        currentVisual.transform.localPosition += Vector3.up * yOffset;

        SetActiveRecursively(currentVisual.transform, true);
        SetLayerRecursively(currentVisual.transform, gameObject.layer);
    }

    private Bounds GetCombinedBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(Vector3.zero, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        return bounds;
    }


    #endregion

    #region UTILITIES

    private void SetLayerRecursively(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        foreach (Transform child in t)
            SetLayerRecursively(child, layer);
    }

    private void SetActiveRecursively(Transform t, bool active)
    {
        t.gameObject.SetActive(active);
        foreach (Transform child in t)
            SetActiveRecursively(child, active);
    }

    public void FreezePhysics()
    {
        if (rb == null) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        frozenBySlot = true;
    }

    public void ReleaseHand()
    {
        if (isLeftHand && leftHand == this)
        {
            leftHand = null;
            if (leftHandCamera != null)
                leftHandCamera.targetTexture = null;
        }
        else if (!isLeftHand && rightHand == this)
        {
            rightHand = null;
            if (rightHandCamera != null)
                rightHandCamera.targetTexture = null;
        }
    }

    public void UnfreezePhysics()
    {
        if (rb == null) return;

        rb.constraints = RigidbodyConstraints.None;
        frozenBySlot = false;
    }

    #endregion
}