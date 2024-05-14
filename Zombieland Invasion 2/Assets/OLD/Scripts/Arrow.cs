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
    }

    private void Update()
    {
        if (isStuck)
        {
            return;
        }

        // Move the arrow forward
        rb.velocity = rb.transform.forward * shootForce;

        // Schedule the arrow to disappear after 4 seconds
        Invoke("Disappear", 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStuck)
        {
            return; // Already stuck, ignore further collisions
        }

        // Stick the arrow to the collided object (assuming it has a Rigidbody)
        isStuck = true;
        rb.isKinematic = true; // Disable physics simulation
        transform.parent = other.transform; // Parent the arrow to the collided object

        // Apply the initial rotation after becoming stuck
        transform.rotation = initialRotation;

        // Optionally add visual or sound effect for sticking
    }

    private void Disappear()
    {
        // Destroy the arrow GameObject
        Destroy(gameObject);
    }
}
