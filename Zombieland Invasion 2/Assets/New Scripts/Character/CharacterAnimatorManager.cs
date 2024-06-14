using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Flags")]
    public bool applyRootMotion = false;

    [Header("Damage Animations")]
    public string lastDamageAnimationPlayed;
    // medium hit 
    [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
    [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

    [SerializeField] string hit_Backwards_Medium_01 = "hit_Backwards_Medium_01";
    [SerializeField] string hit_Backwards_Medium_02 = "hit_Backwards_Medium_02";

    [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
    [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

    [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";
    [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();

    // ping hit
    [SerializeField] string hit_Forward_Ping_01 = "hit_Forward_Ping_01";
    [SerializeField] string hit_Forward_Ping_02 = "hit_Forward_Ping_02";

    [SerializeField] string hit_Backwards_Ping_01 = "hit_Backwards_Ping_01";
    [SerializeField] string hit_Backwards_Ping_02 = "hit_Backwards_Ping_02";

    [SerializeField] string hit_Left_Ping_01 = "hit_Left_Ping_01";
    [SerializeField] string hit_Left_Ping_02 = "hit_Left_Ping_02";

    [SerializeField] string hit_Right_Ping_01 = "hit_Right_Ping_01";
    [SerializeField] string hit_Right_Ping_02 = "hit_Right_Ping_02";

    public List<string> forward_Ping_Damage = new List<string>();
    public List<string> backward_Ping_Damage = new List<string>();
    public List<string> left_Ping_Damage = new List<string>();
    public List<string> right_Ping_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        // ping
        forward_Ping_Damage.Add(hit_Forward_Ping_01);
        forward_Ping_Damage.Add(hit_Forward_Ping_02);

        backward_Ping_Damage.Add(hit_Backwards_Ping_01);
        backward_Ping_Damage.Add(hit_Backwards_Ping_02);

        left_Ping_Damage.Add(hit_Left_Ping_01);
        left_Ping_Damage.Add(hit_Left_Ping_02);

        right_Ping_Damage.Add(hit_Right_Ping_01);
        right_Ping_Damage.Add(hit_Right_Ping_02);

        // medium
        forward_Medium_Damage.Add(hit_Forward_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Medium_02);

        backward_Medium_Damage.Add(hit_Backwards_Medium_01);
        backward_Medium_Damage.Add(hit_Backwards_Medium_02);

        left_Medium_Damage.Add(hit_Left_Medium_01);
        left_Medium_Damage.Add(hit_Left_Medium_02);

        right_Medium_Damage.Add(hit_Right_Medium_01);
        right_Medium_Damage.Add(hit_Right_Medium_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach(var item in animationList)
        {
            finalList.Add(item);
        }

        // check if we have already played this damage animation so it doesint repeat
        finalList.Remove(lastDamageAnimationPlayed);

        // check the list for null entries and remove them 
        for (int i = finalList.Count - 1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }
    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {

        float snappedHorizontal;
        float snappedVertical;

        // this if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1

        if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
        {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalMovement > 0.5f && horizontalMovement <= 1)
        {
            snappedHorizontal = 1;
        }
        else if(horizontalMovement < 0 && horizontalMovement >= -0.5f)
        {
            snappedHorizontal = -0.5f;
        }
        else if(horizontalMovement < -0.5f && horizontalMovement >= -1)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        //this if chain will round the vertical movement to -1, -0.5, 0, 0.5, or 1
        if (verticalMovement > 0 && verticalMovement <= 0.5f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.5f && verticalMovement <= 1)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement >= -0.5f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.5f && verticalMovement >= -1)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isSprinting )
        {
            snappedVertical = 2;
        }

        character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        //Debug.Log("Playing animation: " + targetAnimation);

        this.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPreformingAction = isPerformingAction;
        character.characterLocomationManager.canRotate = canRotate;
        character.characterLocomationManager.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(
        WeaponItem weapon,
        AttackType attackType, 
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false,
        bool canMove = false)
    {

        // keep track of last attack performed (for combos)
        // keep tharck of current attack type (light, heavy)
        // update animation set to current weapon animations
        // decide if our attack can be parried (to do)
        // tell the network our (isattacking) floag is active (for counter damage)
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
        UpdateAnimatorController(weapon.weaponAnimator);

        character.characterAnimatorManager.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPreformingAction = isPerformingAction;
        character.characterLocomationManager.canRotate = canRotate;
        character.characterLocomationManager.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public void UpdateAnimatorController(AnimatorOverrideController weaponController)
    {
        character.animator.runtimeAnimatorController = weaponController;
    }
}
