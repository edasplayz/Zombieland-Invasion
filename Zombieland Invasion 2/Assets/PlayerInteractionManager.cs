using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager player;

    private List<Interactable> currentInteractableActions; // dont serialze

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
    }

    private void FixedUpdate()
    {
        if(!player.IsOwner)
        {
            return;
        }

        // if our ui menu is not open and we dont have a pop up (current interaction message) check for interactable
        if(!PlayerUiManager.instance.menuWindowIsOpen && !PlayerUiManager.instance.popUpWindowIsOpen)
        {
            CheckForInteractable();
        }
    }

    private void CheckForInteractable()
    {
        if(currentInteractableActions.Count == 0)
        {
            return;
        }

        if (currentInteractableActions[0] == null)
        {
            currentInteractableActions.RemoveAt(0); // if the current interactable item at position 0 become null (removed from game) we remove position 0 from the list
            return;
        }

        // if we have an interactable action and have not notified our player we do se here
        if (currentInteractableActions[0] != null)
        {
            PlayerUiManager.instance.playerUiPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }
    }

    private void RefreshInteractionList()
    {
        for(int i = currentInteractableActions.Count - 1; i > -1; i--)
        {
            if (currentInteractableActions[i] == null)
            {
                currentInteractableActions.RemoveAt(i);
            }
        }
    }

    public void AddInteractionToList(Interactable interactableObject)
    {
        RefreshInteractionList();

        if (!currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Add(interactableObject);
        }
    }

    public void RemoveInteractionFromList(Interactable interactableObject)
    {
        

        if (currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Remove(interactableObject);
        }

        RefreshInteractionList();
    }

    public void Interact()
    {
        if(currentInteractableActions == null)
        {
            return;
        }

        if (currentInteractableActions[0] != null)
        {
            currentInteractableActions[0].Interact(player);
            RefreshInteractionList();
        }
    }


}
