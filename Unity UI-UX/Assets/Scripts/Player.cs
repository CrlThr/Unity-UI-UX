using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Rigidbody rigidBody = null;

    [Header("Transforms")]
    [SerializeField] private Transform root = null;
    [SerializeField] private Transform head = null;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 10f;

    private Vector3 input = Vector3.zero;
    private float targetYaw;
    private float currentYaw;


    [SerializeField] private UIObject Objui;

    private void Reset() => rigidBody = GetComponent<Rigidbody>();

    public void Player_OnMove(CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        input.z = input.y;
        input.y = 0;
    }

    public void Player_OnLook(CallbackContext context)
    {
        if (!context.performed) return;
        float x = context.ReadValue<Vector2>().x;
        targetYaw += x * 90f;
    }


    private void LateUpdate()
    {
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * rotateSpeed);
        root.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    private void FixedUpdate()
    {
        Vector3 move = root.forward * input.z + root.right * input.x;
        rigidBody.linearVelocity = move * speed + new Vector3(0, rigidBody.linearVelocity.y, 0);
    }

}
