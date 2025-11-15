using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public HapticEventPulse hapticPulse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HapticSurface"))
        {
            if (hapticPulse != null)
                hapticPulse.PlayHaptic();
            else
                Debug.LogWarning("No HapticEventPulse assigned on " + gameObject.name);
        }
    }
}