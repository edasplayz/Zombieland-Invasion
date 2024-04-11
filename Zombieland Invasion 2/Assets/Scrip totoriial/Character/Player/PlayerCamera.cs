using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20;
    [SerializeField] float minimumViewableAngle = -50;
    [SerializeField] float maximumViewableAngle = 50;
    [SerializeField] float maximumLockOnDistance = 20;


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

    public void HandleLocatingLockOnTargets()
    {
        float shortestDistance = Mathf.Infinity; // will be used to determine the target closest to us
        float shortestDistanceOfRightTarget = Mathf.Infinity; // will be used to determine shortest distance on on axis to the right of current target + (closest to the right of current target )
        float shortestDistanceOfLeftTarget = -Mathf.Infinity; // will be used to determine shortest distance on on axis to the left of current target - (closest to the Left of current target )

        // to do use a layermask
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers()); 

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if(lockOnTarget != null)
            {
                // heck if they are within our field of view 
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                // if target is dead check the next potensial target
                if(lockOnTarget.isDead.Value)
                {
                    continue;
                }
                // if target is us check the next potensial target
                if(lockOnTarget.transform.root == player.transform.root)
                {
                    continue;
                }
                // if target is to far away check the next potensial target
                if (distanceFromTarget > maximumLockOnDistance)
                {
                    continue;
                }

                if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                {
                    RaycastHit hit;

                    // todo addd layer mask for enviromental layer only
                    if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position, 
                        lockOnTarget.characterCombatManager.lockOnTransform.position, 
                        out hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                    {
                        // we hit something we cannot see our lock on target
                        continue;
                    }
                    else
                    {
                        Debug.Log("WE made it");
                    }
                }

            }
        }
    }
}
