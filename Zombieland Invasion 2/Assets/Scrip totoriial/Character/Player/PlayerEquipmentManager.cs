using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        // get our slots
        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeaponsOnBothHands();
    }

    private void InitializeWeaponSlots() 
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach(var weaponSlot in weaponSlots)
        {
            if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    // right weapon

    public void LoadRightWeapon()
    {
        if(player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // remove the old weapon
            rightHandSlot.UnloadWeapon();

            // bring in the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            // assign weapon damage, to its collider
        }
    }

    public void SwitchRightWeapon()
    {
        if(!player.IsOwner)
        {
            return;
        }
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        // check if we have another weapon besides our main weapon, if we do, never swap to unarmed, rotate between weapon 1 and 2 
        // if we dont swap to unarmed then skip the other empty slot and swap back, do not process both empty slots before returning to main weapon

        WeaponItem selectedWeapon = null;

        // disable two handling if we are two handling
        // check our weapon index (we have 3 slots so thats 3 possible numbers)

        // add one to our index to switch to the next potiancial weapon 
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        // if our index is out of bounds, reset it to position #1 (0)
        if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            // we check if we are holding more then one weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if(firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                    
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            // check to see if this is not the "unarmed" weapon
            // if the next potencial weapon does not equal the unarmed weapon 
            if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                // assign the network weapon ID so it switches for all connected clients
                player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                return;
            }
        }

        if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
        
     }

    //left weapon

    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
        {
            return;
        }
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        // check if we have another weapon besides our main weapon, if we do, never swap to unarmed, rotate between weapon 1 and 2 
        // if we dont swap to unarmed then skip the other empty slot and swap back, do not process both empty slots before returning to main weapon

        WeaponItem selectedWeapon = null;

        // disable two handling if we are two handling
        // check our weapon index (we have 3 slots so thats 3 possible numbers)

        // add one to our index to switch to the next potiancial weapon 
        player.playerInventoryManager.leftHandWeaponIndex += 1;

        // if our index is out of bounds, reset it to position #1 (0)
        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            // we check if we are holding more then one weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }

                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
        {
            // check to see if this is not the "unarmed" weapon
            // if the next potencial weapon does not equal the unarmed weapon 
            if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                // assign the network weapon ID so it switches for all connected clients
                player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }
    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // remove the old weapon
            leftHandSlot.UnloadWeapon();

            // bring in the new weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            // assign weapon damage, to its collider
        }
    }


    // damage colliders 

    public void OpenDamageCollider()
    {
        // open right weapon damage collider
        if(player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
        // open left weapon damage collider
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }

        // play whoosh sfx
    }

    public void CloseDamageCollider()
    {
        // open right weapon damage collider
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        // open left weapon damage collider
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }

        // play whoosh sfx
    }

}
