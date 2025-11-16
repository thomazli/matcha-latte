using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class UdpVrLocomotion : MonoBehaviour
{
    [Header("UDP Source")]
    public UDPReceiver udp;

    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    //public float gravity = -9.81f;
    public float gravity = 0f;
    public float smoothing = 0.25f;   // 0 = no smoothing, 1 = max smoothing
    public float deadzone = 0.08f;    // ignore tiny noisy inputs

    [Header("Height Adjustment")]
    public bool autoAdjustHeight = true;
    public float minHeight = 1.0f;
    public float maxHeight = 2.2f;
    public float heightOffset = 0.15f; // extra room so head does not clip

    [Header("Optional Debug")]
    public bool logInput = true;

    private CharacterController controller;
    private Transform head;

    private float smoothedX;
    private float smoothedZ;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        head = GetComponentInChildren<Camera>().transform;

        if (udp == null)
            Debug.LogError("UDPReceiver not assigned in inspector!");
    }

    void Update()
    {
        // Get latest joystick values from UDP
        float x = udp.valueX;
        float z = udp.valueZ;

        // Deadzone filter
        if (Mathf.Abs(x) < deadzone) x = 0;
        if (Mathf.Abs(z) < deadzone) z = 0;

        // Input smoothing (low-pass filter)
        smoothedX = Mathf.Lerp(smoothedX, x, 1f - smoothing);
        smoothedZ = Mathf.Lerp(smoothedZ, z, 1f - smoothing);

        //if (logInput)
            //Debug.Log($"EMG Input: X={smoothedX:F3}, Z={smoothedZ:F3}");

        // Calculate movement direction based on head forward/right
        Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
        Vector3 right   = new Vector3(head.right.x,   0, head.right.z).normalized;

        Vector3 move = (forward * smoothedZ + right * smoothedX) * moveSpeed;

        // Apply gravity
        if (controller.isGrounded)
            verticalVelocity = 0f; // slight downward force for grounding
        else
            verticalVelocity += gravity * Time.deltaTime;

        //move.y = verticalVelocity;
        move.y = 0f; // Disable vertical movement

        // Move character
        controller.Move(move * Time.deltaTime);

        // Adjust capsule height so player can crouch/stand
        if (autoAdjustHeight)
            AdjustHeightToHead();
    }

    void AdjustHeightToHead()
    {
        float headLocalY = head.localPosition.y;
        float newHeight = Mathf.Clamp(headLocalY + heightOffset, minHeight, maxHeight);

        controller.height = newHeight;
        controller.center = new Vector3(0, newHeight / 2f, 0);
    }
}