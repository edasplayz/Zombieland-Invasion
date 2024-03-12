using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] Transform cameraPivotTransform;


    // change those to tweak camera settngs
    [Header("Camra Settings")]
    private float cameraSmoothSpeed = 1; // the biger the longer it takes camera to move towards player
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30; // the lowest poin you can look down
    [SerializeField] float maximumPivot = 60; // the hiest point you can look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;


    // just displaes camera values
    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; //used for camera collisions (moves the cameras object to this position upon colliding)
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition; // values used for camera position
    private float targetCameraZPosition; // values used for camera position


    private void Awake()
    {
        // there can only be one instance of this script at one time, if another exist, destroy it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        
        if(player != null)
        {
            // fallow the player
            HandleFallowTarget();
            // roatate areound with player
            HandleRotations();
            // colide with objects
            HandleCollisions();
        }
        
    }

    private void HandleFallowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;

    }

    private void HandleRotations()
    {
        // if lopced on force rotation towards target
        // else rotate normaly

        // normal rotations
        // rotate left and right based of horizontal mevement on the right joystick
        leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        // rotate up and down based on vertical movement on the right joystick
        upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        // clamp the up and down look angle between a min and max value 
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);


        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // rotate this gameobject left and right
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // rotate the pivot gamneobject up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        // direction for collsiion check
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // we check if there is an object in front of the camera
        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // if there is we get our distance from it 
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // we then equate our target z position to the fallowing
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        // if our target position is less then our collsiion radius, we subtrack our collsion radius (making it snap back)
        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }
        // we then apply our final position using a lerp over a time of 0.2f
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
