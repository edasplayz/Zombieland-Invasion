using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{
    public float gravityMultiplier = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Disable the Rigidbody's use of global gravity. This will ensure that our custom gravity is applied correctly.
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Apply our custom gravity to the Rigidbody.
        rb.AddForce(Physics.gravity * gravityMultiplier);
    }
}
