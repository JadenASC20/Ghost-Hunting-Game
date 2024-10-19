using UnityEngine;

public class Wall : MonoBehaviour
{
    // Specify how far to push the object back
    public float pushBackDistance = 0.1f;
    public float pushBackDuration = 0.1f; // Time to push back
    public LayerMask defaultLayer; // Set this in the inspector to specify the layer

    private Collider currentCollider;
    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private float elapsedTime;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with Wall: " + other.gameObject.name);

        // Check if the colliding object's layer is the default layer
        if ((defaultLayer & (1 << other.gameObject.layer)) != 0)
        {
            currentCollider = other;
            originalPosition = currentCollider.transform.position;
            targetPosition = originalPosition + (originalPosition - transform.position).normalized * pushBackDistance;
            elapsedTime = 0f;
        }
    }

    private void Update()
    {
        if (currentCollider != null)
        {
            // Smoothly push back the object
            if (elapsedTime < pushBackDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / pushBackDuration;
                currentCollider.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            }
            else
            {
                // Reset the currentCollider after the push back is complete
                currentCollider = null;
            }
        }
    }
}
