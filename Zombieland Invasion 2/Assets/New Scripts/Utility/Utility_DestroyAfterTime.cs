using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility_DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilDestroyed = 1;

    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }
}
