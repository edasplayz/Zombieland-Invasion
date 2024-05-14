using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrapProjectileShoot : MonoBehaviour
{
    //arrow prfab
    [SerializeField] private GameObject arrow;
    // shoot position
    [SerializeField] private Transform shootTransform;

    // Time delay between spawns (in seconds)
    [SerializeField] private float spawnDelay = 1.0f;

    private void Start()
    {
        InvokeRepeating(nameof(ShootProjectile), 0.0f, spawnDelay);
    }

    private void ShootProjectile()
    {
        GameObject go = Instantiate(arrow, shootTransform.position, shootTransform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
