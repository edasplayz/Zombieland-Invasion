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

    private void Update()
    {
        GameObject go = Instantiate(arrow, shootTransform.position, shootTransform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
