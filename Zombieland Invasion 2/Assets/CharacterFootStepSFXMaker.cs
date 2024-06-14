using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
    // this need to be fixed

    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootStepSFX = false;
    [SerializeField] float distanceToGround = 0.05f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponentInParent<CharacterManager>();
    }

     private void FixedUpdate()
     {
         CheckForFootSteps();

     }

     private void CheckForFootSteps()
     {
         if(character == null)
         {
             return;
         }

         if(!character.characterNetworkManager.isMoving.Value )
         {
             return;
         }

         RaycastHit hit;

         if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.Instance.GetEnviroLayers()))
         {
             hasTouchedGround = true;

             if(!hasPlayedFootStepSFX)
             {
                 steppedOnObject = hit.transform.gameObject;
             }
         }
         else
         {
             hasTouchedGround= false;
             hasPlayedFootStepSFX = false;
             steppedOnObject= null;
         }

         if(hasTouchedGround && !hasPlayedFootStepSFX)
         {
             hasPlayedFootStepSFX = true;
             PlayFootStepSoundFX();
         }


     }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (character == null || audioSource == null)
        {
            return;
        }

        // Check if the collision is with layers defined as environment layers
        if ((collision.gameObject.layer & WorldUtilityManager.Instance.GetEnviroLayers().value) != 0)
        {
            // Check if the character is moving and footstep sound hasn't been played yet
            if (character.characterNetworkManager.isMoving.Value && !hasPlayedFootStepSFX)
            {
                PlayFootStepSoundFX();
                hasPlayedFootStepSFX = true;
            }
        }
    }*/

    private void PlayFootStepSoundFX()
    {
        
        character.characterSoundFXManager.PlayFootStepSoundFX();
    }

}
