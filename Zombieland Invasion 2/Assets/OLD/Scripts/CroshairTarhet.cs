using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroshairTarhet : MonoBehaviour
{

    Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;
    public Transform rayCastOrigin;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
   void Update()
    {
        ray.origin = rayCastOrigin.transform.position;
        ray.direction = rayCastOrigin.transform.forward;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
