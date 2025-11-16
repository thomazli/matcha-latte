using UnityEngine;

/// <summary>
/// Changes the color of an object when touched by a hand in VR.
/// Attach this to objects (like cubes) that you want to change color on touch.
/// Requires a Renderer component and a Collider set to "Is Trigger".
/// </summary>
public class ChangeColorOnTouch : MonoBehaviour
{
    [Header("Color Settings")]
    [Tooltip("Color to change to when touched by a hand")]
    public Color touchColor = Color.red;

    [Tooltip("If true, resets to original color when hand stops touching")]
    public bool resetColorOnExit = false;

    [Header("Detection Settings")]
    [Tooltip("Tags to identify hand colliders (can add custom tags here)")]
    public string[] handTags = new string[] { "Hand" };

    // Store the original color to reset back to
    private Color originalColor;

    // Reference to this object's renderer
    private Renderer cubeRenderer;

    // Track whether currently being touched
    private bool isTouched = false;

    private void Start()
    {
        // Get the renderer component and store original color
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            originalColor = cubeRenderer.material.color;
        }
        else
        {
            Debug.LogError("ChangeColorOnTouch requires a Renderer component on " + gameObject.name);
        }
    }

    /// <summary>
    /// Called when another collider enters this object's trigger zone
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a hand
        if (IsHand(other))
        {
            if (cubeRenderer != null && !isTouched)
            {
                // Change to the touch color
                cubeRenderer.material.color = touchColor;
                isTouched = true;
                Debug.Log($"Hand '{other.gameObject.name}' touched {gameObject.name}");
            }
        }
    }

    /// <summary>
    /// Called when another collider exits this object's trigger zone
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to a hand and reset is enabled
        if (IsHand(other) && resetColorOnExit)
        {
            if (cubeRenderer != null)
            {
                // Reset back to original color
                cubeRenderer.material.color = originalColor;
                isTouched = false;
                Debug.Log($"Hand '{other.gameObject.name}' left {gameObject.name}");
            }
        }
    }

    /// <summary>
    /// Determines if a collider belongs to a hand using multiple detection methods
    /// </summary>
    /// <param name="collider">The collider to check</param>
    /// <returns>True if the collider is part of a hand</returns>
    private bool IsHand(Collider collider)
    {
        // Method 1: Check by tag (e.g., "Hand")
        foreach (string tag in handTags)
        {
            if (collider.CompareTag(tag))
            {
                return true;
            }
        }

        // Method 2: Check if on a "Hand" layer
        if (collider.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            return true;
        }

        // Method 3: Check if name contains "Hand" (fallback method)
        if (collider.gameObject.name.ToLower().Contains("hand"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Manually set the object to a specific color (optional helper method)
    /// </summary>
    /// <param name="newColor">The color to change to</param>
    public void SetColor(Color newColor)
    {
        if (cubeRenderer)
            cubeRenderer.material.color = newColor;
    }

    /// <summary>
    /// Manually reset the object to its original color (optional helper method)
    /// </summary>
    public void ResetColor()
    {
        if (cubeRenderer)
            cubeRenderer.material.color = originalColor;
    }
}
