using UnityEngine;
using UnityEngine.InputSystem; // Wajib untuk Input System baru

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 10f;

    [Header("Gravity")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity = 0f;

    void Start()
    {
        // Mengambil komponen Character Controller yang ada di objek ini
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("Karakter butuh komponen Character Controller! Tambahkan dulu di Inspector.");
        }
    }

    void Update()
    {
        // Cek apakah keyboard terdeteksi
        if (Keyboard.current == null) return;

        // 1. Baca Input (WASD atau Panah)
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;

        // 2. Hitung arah pergerakan
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. Logika Gravity (Agar tidak melayang)
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Tempel ke lantai
        }
        verticalVelocity += gravity * Time.deltaTime;

        // 4. Eksekusi Pergerakan
        Vector3 finalMove = new Vector3(moveDirection.x * moveSpeed, verticalVelocity, moveDirection.z * moveSpeed);
        controller.Move(finalMove * Time.deltaTime);

        // 5. Rotasi Karakter (Menghadap ke arah jalan)
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}