using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SiteOfGraceInteractable : Interactable
{
    [Header("Site Of Grace Info")]
    [SerializeField] int siteOfGraceID;
    public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("VFX")]
    [SerializeField] GameObject activatedParticles;

    [Header("Interaction Text")]
    [SerializeField] string unactivatedInteractionText = "Activate Check Point";
    [SerializeField] string activatedInteractionText = "Rest";

    protected override void Start()
    {
        base.Start();

        if (IsOwner)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
            {
                isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
            }
            else
            {
                isActivated.Value = false;
            }
        }

        if(isActivated.Value)
        {
            interactableText = activatedInteractionText;
        }
        else
        {
            interactableText = unactivatedInteractionText;
        }

        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if we join when the status has already changed we force the onchange function to run here upon joining
        if(!IsOwner)
        {
            OnIsActivatedChanged(false, isActivated.Value);
        }

        isActivated.OnValueChanged += OnIsActivatedChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActivated.OnValueChanged -= OnIsActivatedChanged;
    }

    private void RestoreSiteOfGrace(PlayerManager player)
    {
        
        isActivated.Value = true;

        //if our save file contains info on this site of grace remove it
        if(WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey (siteOfGraceID))
        {
            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);
        }

        // then re-add it with the value of "true" (is activated)
        WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true );

        player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);
        //hide weapon models whilist playing animation if you desire


        // sends a pop up
        PlayerUiManager.instance.playerUiPopUpManager.SendGraceRestoredPopUp("Check Poin Restored");

        StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        
    }

    private void RestAtSiteOfGrace(PlayerManager player)
    {
        Debug.Log("Resting");

        // temporaly code section
        interactableCollider.enabled = true; // temporarily re_enabling the collider here until we add the menu so you can respawn monsters indefinitely
        player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
        player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;
        // refill flask (to do)
        // update/force move quest characters (to do)

        // reset monsters/character locations
        WorldAIManager.instance.ResetAllCharacters();
    }

    private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
    {
        yield return new WaitForSeconds(2); // this should give enought time for animation to play and the pop up to begin fading
        interactableCollider.enabled = true;
    }

    private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
    {
        if (isActivated.Value)
        {
            // play some fx here if you like or enable a light or something to indicate this check point in on
            activatedParticles.SetActive(true);

            
            interactableText = activatedInteractionText;
            
            
        }
        else
        {
            interactableText = unactivatedInteractionText;
        }
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if(!isActivated.Value) 
        { 
            RestoreSiteOfGrace(player);
        
        }
        else
        {
            RestAtSiteOfGrace(player);
        }
    }
}
