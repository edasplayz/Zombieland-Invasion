using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        // Sunaikins objekta po 3 sekundziu
        Destroy(gameObject, 3.0f);
    }
}
