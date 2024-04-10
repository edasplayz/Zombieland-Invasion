using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastDamageAnimationPlayed;

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

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
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
    public void UpdateAnimatorMovementParameters(float horizontalValues, float verticalValues, bool isSprinting)
    {

        float horizontalAmount = horizontalValues;
        float verticalAmount = verticalValues;


        if(isSprinting )
        {
            verticalAmount = 2;
        }

        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        Debug.Log("Playing animation: " + targetAnimation);

        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // can be used to stop character form attemting new action
        // form exaple if you get damaged and begin performing a damage animation
        // this flag will turn true if you are stunned 
        // we can then check for this before attempting new actions
        character.isPreformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(AttackType attackType, 
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false,
        bool canMove = false)
    {

        // keep track of last attack performed (for combos)
        // keep tharck of current attack type (light, heavy, ect)
        // update animation set to current weapon animations
        // decide if our attack can be parried
        // tell the network our (isattacking) floag is active (for counter damage ect)
        character.characterCombatManager.currentAttackType = attackType;
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPreformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // tell the server/host we player an animation, and to play that animation for everyone else present 
        character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
