using UnityEngine;

public class BowSystem : MonoBehaviour
{
    public GameObject bowString;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;
    public float maxPullDistance = 2f;

    private bool isPulling = false;
    private float pullDistance = 0f;

    void Update()
    {
        HandleInput();

        if (isPulling)
        {
            UpdatePullDistance();
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPulling();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ShootArrow();
        }
    }

    void StartPulling()
    {
        isPulling = true;
    }

    void UpdatePullDistance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            pullDistance = Mathf.Min(Vector3.Distance(bowString.transform.position, hit.point), maxPullDistance);
            // You can use this pullDistance value to visualize the bowstring stretch or apply some scaling to the bowstring.
        }
    }

    void ShootArrow()
    {
        isPulling = false;

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

        // Apply force to the arrow based on the pull distance
        Vector3 shootDirection = arrowSpawnPoint.forward;
        arrowRigidbody.velocity = shootDirection * arrowSpeed * (pullDistance / maxPullDistance);

        // Reset pullDistance for the next shot
        pullDistance = 0f;
    }
}
