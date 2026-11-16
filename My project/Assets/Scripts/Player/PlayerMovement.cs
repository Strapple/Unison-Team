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

    [Header("Audio")]
    public AudioSource stepAudio; // <-- TAMBAHAN: Tarik AudioSource karakter ke sini

    [HideInInspector] public bool canMove = true;

    private CharacterController controller;
    private Animator anim;
    private float verticalVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (controller == null) Debug.LogError("Karakter butuh komponen Character Controller!");
        if (anim == null) Debug.LogWarning("Animator tidak ditemukan!");
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        bool isRunning = false;

        if (canMove && Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;

            isRunning = Keyboard.current.shiftKey.isPressed;
        }

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
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
            if (moveDirection != Vector3.zero) animSpeed = isRunning ? 2f : 1f;
            anim.SetFloat("Speed", animSpeed);
        }

        // LOGIKA SFX LANGKAH KAKI
        if (stepAudio != null)
        {
            if (moveDirection != Vector3.zero && controller.isGrounded && canMove)
            {
                if (!stepAudio.isPlaying) stepAudio.Play();
            }
            else
            {
                if (stepAudio.isPlaying) stepAudio.Stop();
            }
        }
    }
}