using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Interactable : MonoBehaviour
{
    // its the object that you can interact for example levers items fog walls levevators

    public string interactableText; // text prompt when entering the interactable collider (pick up item pull lever ect)
    [SerializeField] protected Collider interactableCollider; // collider that checks for player interactable
    [SerializeField] protected bool hostOnlyInteractable = true; // when enabled object cannot be interacted with by coop player

    protected virtual void Awake()
    {
        // check if its null in some cases you want to manualy asign a collidr as a child object (depending on interactable)
        if(interactableCollider == null)
        {
            interactableCollider = GetComponent<Collider>();
        }


    }

    protected virtual void Start()
    {

    }

    public virtual void Interact(PlayerManager player)
    {
        Debug.Log("You Have Interacted!");

        if(!player.IsOwner)
        {
            return;
        }

        // remove the interaction from the player
        interactableCollider.enabled = false;
        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUiManager.instance.playerUiPopUpManager.CloseAllPopUpWindows();

        
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if(player != null)
        {
            if(!player.playerNetworkManager.IsHost && hostOnlyInteractable)
            {
                return;
            }
            if(!player.IsOwner)
            {
                return;
            }

            // pass the interactable to the player
            player.playerInteractionManager.AddInteractionToList(this);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
            {
                return;
            }
            if (!player.IsOwner)
            {
                return;
            }

            // remove the interaction from the player 
            player.playerInteractionManager.RemoveInteractionFromList(this);

            
            PlayerUiManager.instance.playerUiPopUpManager.CloseAllPopUpWindows();
            
        }
    }

}
