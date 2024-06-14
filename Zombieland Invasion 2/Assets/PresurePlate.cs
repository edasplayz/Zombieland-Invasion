using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    public GameObject objectToActivate; 

    [SerializeField] private bool disableOnTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is colliding with an object on the "Character" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            // Activate the GameObject
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(!disableOnTrigger);
            }
        }
    }
}
