using System.Diagnostics;
using UnityEngine;

public class ImpactReplacement : MonoBehaviour
{
    // Public field for setting the impact force threshold
    public float impactForceThreshold = 10f;

    // Public field for specifying the replacement prefab
    public GameObject replacementPrefab;

    // Called when the object collides with another collider
    void OnCollisionEnter(Collision collision)
    {
        // Get the collision impact force
        float impactForce = collision.impulse.magnitude;

        // Check if the impact force exceeds the threshold value
        if (impactForce >= impactForceThreshold)
        {
            // Replace the object with the specified prefab
            ReplaceWithPrefab();
        }
    }

    // Method to replace the object with the specified prefab
    void ReplaceWithPrefab()
    {
        if (replacementPrefab != null)
        {
            // Instantiate the replacement prefab at the same position and rotation
            GameObject replacementObject = Instantiate(replacementPrefab, transform.position, transform.rotation);

            // Destroy the current object
            Destroy(gameObject);

            // Optionally, you can transfer any necessary components or data from the old object to the new one here
        }
    }
}
