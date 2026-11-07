using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed;
    public float runSpeed;
    public float rotationSpeed;

    [Header("Gravity")]
    public float gravity;

    private CharacterController controller;
    private Animator anim;
    private float verticalVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (controller == null)
            Debug.LogError("Karakter butuh komponen Character Controller!");
        if (anim == null)
            Debug.LogWarning("Animator tidak ditemukan di karakter ini atau child-nya!");
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Keyboard.current.shiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = new Vector3(moveDirection.x * currentSpeed, verticalVelocity, moveDirection.z * currentSpeed);
        controller.Move(finalMove * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (anim != null)
        {
            float animSpeed = 0f;
            if (moveDirection != Vector3.zero)
            {
                animSpeed = isRunning ? 2f : 1f;
            }
            anim.SetFloat("Speed", animSpeed);
        }
    }
}