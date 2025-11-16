using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 2f;     // Forward/backward speed (units/sec)
    public float turnSpeed = 90f;    // Rotation speed (degrees/sec)
    public Transform forwardSource;  // What defines forward; usually the object itself or camera

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from rotating the object
    }

    void FixedUpdate()
    {
        if (forwardSource == null)
            forwardSource = transform;

        // Handle rotation
        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        if (turnInput != 0f)
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // Handle forward/backward movement
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput = -1f;

        if (moveInput != 0f)
        {
            Vector3 forward = forwardSource.forward;
            forward.y = -1f; // optional: prevent moving up/down
            forward.Normalize();

            Vector3 targetPos = rb.position + forward * moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }
}