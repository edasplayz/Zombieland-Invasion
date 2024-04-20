using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
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

    private void PlayFootStepSoundFX()
    {
        // here you could play a different sfx depending on the layer of the ground or a tag or such (snow, wood, stone ect)
        // method 2
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomFootStepSoundBasedOnGround(steppedOnObject, character));

        // method 2 (simple)
        character.characterSoundFXManager.PlayFootStepSoundFX();
    }

}
