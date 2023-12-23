using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    private Quaternion initialRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
        {
            return;
        }
        else
        {
            targetHit = true;
        }

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Store the parent transform before making it a child
        Transform parentTransform = transform.parent;

        // Make the arrow a child of the target
        transform.SetParent(collision.transform, true);

        // Offset the arrow slightly to avoid z-fighting
        transform.position += transform.forward * 0.01f;

        // Apply the initial rotation after becoming a child
        transform.rotation = initialRotation;

        // Restore the parent transform
        transform.SetParent(parentTransform);
    }
}
