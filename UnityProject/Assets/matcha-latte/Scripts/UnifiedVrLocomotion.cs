using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class UnifiedVrLocomotion : MonoBehaviour
{
    [Header("References")]
    public UDPReceiver udp;                // Optional UDP input
    private CharacterController controller;
    private Transform head;

    // ---------------- Movement ----------------
    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float gravity = -9.81f;

    [Tooltip("Ignore tiny noisy inputs")]
    public float deadzone = 0.08f;

    [Range(0f, 1f)]
    [Tooltip("0 = raw, 1 = heavy smoothing")]
    public float udpSmoothing = 0.25f;

    [Header("Keyboard Input")]
    public bool enableKeyboard = true;
    public float keyboardSpeed = 2.5f;

    [Header("Turning")]
    public float turnSpeed = 120f;  // degrees per second
    public bool enableTurn = true;


    // ---------------- Height ----------------
    [Header("Height Adjustment")]
    public bool autoAdjustHeight = true;
    public float minHeight = 1.0f;
    public float maxHeight = 2.2f;
    public float heightOffset = 0.15f;

    // ---------------- Debug ----------------
    public bool logUdp = false;

    // Internal movement state
    private float smoothedX = 0f;
    private float smoothedZ = 0f;
    private Vector3 velocity = Vector3.zero;

    // ================================================================
    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Find Oculus head camera
        head = transform.Find("TrackingSpace/CenterEyeAnchor");

        if (head == null)
        {
            Debug.LogError("Could not find CenterEyeAnchor. Is this an OVRCameraRig?");
            enabled = false;
            return;
        }

        if (udp == null)
        {
            Debug.LogWarning("No UDPReceiver assigned — UDP movement disabled.");
        }
    }

    // ================================================================
    void Update()
    {
        UpdateCharacterHeight();

        Vector3 moveInput = GetMovementInput();

        ApplyMovement(moveInput);

        if (enableTurn)
            HandleRotation();

    }

    // ================================================================
    Vector3 GetMovementInput()
    {
        Vector3 move = Vector3.zero;

        float ux = 0f;
        float uz = 0f;

        // -------- UDP INPUT --------
        if (udp != null)
        {
            ux = udp.valueX;
            uz = udp.valueZ;

            // Deadzone
            if (Mathf.Abs(ux) < deadzone) ux = 0f;
            if (Mathf.Abs(uz) < deadzone) uz = 0f;

            // Low-pass smoothing
            smoothedX = Mathf.Lerp(smoothedX, ux, 1f - udpSmoothing);
            smoothedZ = Mathf.Lerp(smoothedZ, uz, 1f - udpSmoothing);

            if (logUdp)
                Debug.Log($"UDP: x={smoothedX:F3}, z={smoothedZ:F3}");
        }

        // -------- KEYBOARD INPUT --------
        if (enableKeyboard)
        {
            float kx = Input.GetAxis("Horizontal"); // A/D
            float kz = Input.GetAxis("Vertical");   // W/S

            smoothedX += kx * keyboardSpeed * 0.25f;
            smoothedZ += kz * keyboardSpeed * 0.25f;
        }

        // -------- HEAD-RELATIVE MOVEMENT --------
        Vector3 forward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
        Vector3 right   = new Vector3(head.right.x,   0, head.right.z).normalized;

        move = (forward * smoothedZ + right * smoothedX) * moveSpeed;

        return move;
    }

    // ================================================================
    void ApplyMovement(Vector3 move)
    {
        // -------- Gravity --------
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -1f;  // keep grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);
    }

    // ================================================================
    void HandleRotation()
    {
        float turnInput = 0f;

        if (Input.GetKey(KeyCode.Q))
            turnInput = -1f;
        else if (Input.GetKey(KeyCode.E))
            turnInput = 1f;

        if (turnInput != 0f)
        {
            float turnAmount = turnSpeed * turnInput * Time.deltaTime;
            transform.Rotate(0, turnAmount, 0);
        }
    }


    // ================================================================
    void UpdateCharacterHeight()
    {
        if (!autoAdjustHeight) return;

        float headLocalY = head.localPosition.y;
        float newHeight = Mathf.Clamp(headLocalY + heightOffset, minHeight, maxHeight);

        controller.height = newHeight;
        controller.center = new Vector3(0, newHeight / 2f, 0);
    }
}
