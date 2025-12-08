using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Rigidbody rigidBody = null;
    private float rotateAmount = 90;

    [Header("Transforms")]
    [SerializeField] private Transform root = null;
    [SerializeField] private Transform head = null;

    private Vector3 input = Vector3.zero;
    private float targetYaw;
    private float currentYaw;

    private void Reset()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    public void Player_OnMove(CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        input.z = input.y;
        input.y = 0;
    }

    public void Player_OnLook(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector2 input = context.ReadValue<Vector2>(); 
        float x = input.x;

        if (x != 0)
            targetYaw += x * rotateAmount;
    }

    private void LateUpdate()
    {
        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * rotateSpeed);
        root.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = root.rotation * (speed * input.normalized);
    }
}
