using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool isStuck;
    private Quaternion initialRotation;
    private Rigidbody rb;
    [SerializeField] float shootForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;

        // Ensure trigger is enabled
        GetComponent<Collider>().isTrigger = true;

        // Apply initial force in the forward direction
        rb.velocity = transform.forward * shootForce;
    }

    private void Update()
    {
        if (isStuck)
        {
            return;
        }

        // Update rotation to match velocity
        transform.rotation = Quaternion.LookRotation(rb.velocity);

        // Schedule the arrow to disappear after 4 seconds
        Invoke("Disappear", 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStuck)
        {
            return; // Already stuck, ignore further collisions
        }

        // Check for CharacterController component
        if (other.GetComponent<CharacterController>() != null)
        {
            // Stick to the object with CharacterController
            isStuck = true;
            rb.isKinematic = true; // Disable physics simulation
            transform.parent = other.transform; // Parent the arrow

            // Apply the initial rotation after becoming stuck
            transform.rotation = initialRotation;

            // Optionally add visual or sound effect for sticking
        }
        else
        {
            // Check for collisions with anything except triggers
            if (!other.isTrigger)  // Check if not a trigger
            {
                // Stick to object (assuming it's not a trigger)
                isStuck = true;
                rb.isKinematic = true; // Disable physics simulation
                                       // Don't parent the arrow (remains in place)
                transform.rotation = initialRotation;

                // Optionally add visual or sound effect for sticking
            }
            // Existing logic for setting velocity (not needed here)
            // else
            // {
            //   // Set velocity before sticking (for non-character colliders)
            //   rb.velocity = rb.transform.forward * shootForce;
            // }
        }
    }

    private void Disappear()
    {
        // Destroy the arrow GameObject
        Destroy(gameObject);
    }
}
