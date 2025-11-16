using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform forwardSource; // usually the camera for VR, or the player object

    void Update()
    {
        if (forwardSource == null)
            forwardSource = transform;

        float moveInput = 0f;

        if (Input.GetKey(KeyCode.W))
            moveInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            moveInput = -1f;

        float turn = 0f;

        if (Input.GetKey(KeyCode.A))
            turn = -1f;
        else if (Input.GetKey(KeyCode.D))
            turn = 1f;

        transform.Rotate(Vector3.up * turn * 90f * Time.deltaTime);

        Vector3 direction = forwardSource.forward;
        direction.y = 0f; // Prevent moving upward/down slopes unintentionally
        direction.Normalize();

        transform.position += direction * moveInput * moveSpeed * Time.deltaTime;
    }
}
