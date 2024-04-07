using UnityEngine;
using UnityEngine.AI;

public class LookAtObject : MonoBehaviour
{
    // Reference to the target object to look at
    public Transform targetObject;
    public Rigidbody playerRb;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (targetObject != null)
        {
            // Get the direction from the player to the target object, ignoring the Y component
            Vector3 direction = targetObject.position - transform.position;
            direction.y = 0f; // Set Y component to 0 to only consider rotation around the Y-axis

            // Rotate the player model to face the target object's direction around the Y-axis
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            }

            // Check if the player is moving
            float speed = playerRb.velocity.magnitude;
            if (speed != 0f)
            {
                // Set the MoveSpeed parameter to 1 when moving
                animator.SetFloat("MoveSpeed", 1f);
            }
            else
            {
                // Set the MoveSpeed parameter to 0 when not moving
                animator.SetFloat("MoveSpeed", 0f);
            }
        }
    }
}
