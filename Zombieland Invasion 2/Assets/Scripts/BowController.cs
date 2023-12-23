using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float maxShootForce = 50f;
    public float chargeRate = 10f;

    [Header("Target Object (Set in Editor)")]
    public Transform targetObject;

    private bool isAiming = false;
    private float currentShootForce;
    private GameObject currentArrow;

    public GameObject arrow;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartAiming();
        }

        if (isAiming)
        {
            Charge();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    void StartAiming()
    {
        isAiming = true;
        currentShootForce = 0f;
        GetComponent<Animator>().SetTrigger("Shoot");
        arrow.SetActive(true);

    }

    void Charge()
    {
        if (isAiming)
        {
            currentShootForce += chargeRate * Time.deltaTime;
            currentShootForce = Mathf.Clamp(currentShootForce, 0f, maxShootForce);
        }
    }

    void Shoot()
    {
        if (isAiming)
        {
            isAiming = false;

            // Play your shoot animation here
            GetComponent<Animator>().SetTrigger("Relese");

            if (targetObject != null)
            {
                currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
                Vector3 targetDirection = (targetObject.position - arrowSpawnPoint.position).normalized;
                currentArrow.transform.forward = targetDirection;

                Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();
                arrowRb.AddForce(currentArrow.transform.forward * currentShootForce, ForceMode.Impulse);
                arrow.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Target object not set. Arrow not fired.");
                Destroy(currentArrow);
            }
        }
    }
}
