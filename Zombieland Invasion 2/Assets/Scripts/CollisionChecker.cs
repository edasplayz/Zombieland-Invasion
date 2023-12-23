using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public GameObject leftCube;
    public GameObject rightCube;
    public GameObject frontCube;
    public GameObject backCube;
    public LayerMask whatIsGround;

    void Update()
    {
        // Check collisions and handle them as needed
        CheckCollision(leftCube, Vector3.left);
        CheckCollision(rightCube, Vector3.right);
        CheckCollision(frontCube, Vector3.forward);
        CheckCollision(backCube, Vector3.back);
    }

    void CheckCollision(GameObject cube, Vector3 direction)
    {
        if (Physics.CheckBox(cube.transform.position, cube.GetComponent<BoxCollider>().size / 2, cube.transform.rotation, whatIsGround))
        {
            Debug.Log(cube.name + " is colliding with an object on the 'whatIsGround' layer in the direction: " + direction);
        }
    }

    // Check if any cube is colliding with an object
    public Vector3 GetCollisionDirection()
    {
        if (IsColliding(leftCube)) return Vector3.left;
        if (IsColliding(rightCube)) return Vector3.right;
        if (IsColliding(frontCube)) return Vector3.forward;
        if (IsColliding(backCube)) return Vector3.back;

        return Vector3.zero;
    }

    // Check if a specific cube is colliding with an object
    private bool IsColliding(GameObject cube)
    {
        return Physics.CheckBox(cube.transform.position, cube.GetComponent<BoxCollider>().size / 2, cube.transform.rotation, whatIsGround);
    }
}
