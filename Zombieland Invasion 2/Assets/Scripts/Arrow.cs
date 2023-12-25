using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool targetHit;
    private Quaternion initialRotation;
    private Rigidbody rb;

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

        // Remove the Collider component
        Destroy(GetComponent<Collider>());

        // Add a FixedJoint component to connect the arrow to the target
        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = collision.rigidbody;

        // Adjust the connectedMassScale and massScale to allow the target to move more freely
        fixedJoint.connectedMassScale = 0.1f;  // Adjust this value as needed
        fixedJoint.massScale = 1.0f;            // Adjust this value as needed

        // Apply the initial rotation after becoming a child
        transform.rotation = initialRotation;

        // Schedule the arrow to disappear after 10 seconds
        Invoke("Disappear", 10f);
    }

    private void Disappear()
    {
        // Destroy the arrow GameObject after 10 seconds
        Destroy(gameObject);
    }
}
