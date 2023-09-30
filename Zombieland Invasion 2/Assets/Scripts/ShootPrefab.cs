using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPrefab : MonoBehaviour
{
    public GameObject prefabToShoot;
    public GameObject targetGameObject;
    public float FireSpeed;

    public float fireRate = 1f; // Shots per second
    private float timeToNextShot = 0f;

    private void Update()
    {
        // Get the direction to the target game object.
        Vector3 directionToTarget = targetGameObject.transform.position - transform.position;

        // Normalize the direction vector to make sure it has a magnitude of 1.
        directionToTarget = directionToTarget.normalized;

        // Check if the time to next shot has elapsed.
        if (Time.time >= timeToNextShot)
        {
            // Instantiate the prefab to shoot at the current position and rotation.
            GameObject projectile = Instantiate(prefabToShoot, transform.position, transform.rotation);

            // Add a force to the prefab in the direction of the target game object.
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.AddForce(directionToTarget * FireSpeed);

            // Update the time to next shot variable.
            timeToNextShot = Time.time + 1f / fireRate;
        }
    }
}
