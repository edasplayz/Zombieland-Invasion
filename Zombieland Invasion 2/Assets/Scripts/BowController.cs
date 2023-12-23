using UnityEngine;
using System.Collections;


public class BowController : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float maxShootForce = 50f;
    public float chargeRate = 10f;
    public float shotCooldown = 1f; // Adjust this value for the desired cooldown time

    [Header("Target Object (Set in Editor)")]
    public Transform targetObject;

    private bool isAiming = false;
    private float currentShootForce;
    private GameObject currentArrow;
    private bool canShoot = true;

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
        if (canShoot)
        {
            isAiming = true;
            currentShootForce = 0f;
            GetComponent<Animator>().SetTrigger("Shoot");
            arrow.SetActive(true);
        }
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

            if (targetObject != null && canShoot)
            {
                currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
                Vector3 targetDirection = (targetObject.position - arrowSpawnPoint.position).normalized;
                currentArrow.transform.forward = targetDirection;

                Rigidbody arrowRb = currentArrow.GetComponent<Rigidbody>();
                arrowRb.AddForce(currentArrow.transform.forward * currentShootForce, ForceMode.Impulse);
                arrow.SetActive(false);

                // Start the cooldown only if not in cooldown
                if (canShoot)
                {
                    StartCoroutine(ShotCooldown());
                }
            }
            else
            {
                Debug.LogWarning("Target object not set or on cooldown. Arrow not fired.");
                Destroy(currentArrow);
            }
        }
    }

    IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
    }


}
