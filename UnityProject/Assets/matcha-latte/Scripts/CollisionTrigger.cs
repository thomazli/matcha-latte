using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public HapticEventPulse hapticPulse; // Assign your haptic script
    public float pulseInterval = 0.05f;  // time between pulses in seconds

    private float lastPulseTime = 0f;

    [Header("Painting")]
    public Material paintedMaterial;  // assign in Inspector

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("HapticSurface"))
        {
            // Only trigger at intervals
            if (Time.time - lastPulseTime >= pulseInterval)
            {
                if (hapticPulse != null)
                    hapticPulse.PlayHaptic();
                else
                    Debug.LogWarning("No HapticEventPulse assigned on " + gameObject.name);

                lastPulseTime = Time.time;
            }

            // paint full material
            Renderer r = other.GetComponent<Renderer>();
            if (r != null && paintedMaterial != null)
            {
                r.material = paintedMaterial;
            }
        }
    } 
}
