using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("Stats Bar")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

    [Header("Boss Health Bar")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarObject;
    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);

    }
    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }
    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        // if the database does not contain a weapon matching the given i.d return

        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
        if(WorldItemDatabase.Instance.GetWeaponByID(weaponID) == null)
        {
            Debug.Log("Item is null");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if(weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // this is where you would check to see if you meet the item requirements if you want to create the warning for not being able to weald it in the ui

        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
        
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        // if the database does not contain a weapon matching the given i.d return

        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
        if (WorldItemDatabase.Instance.GetWeaponByID(weaponID) == null)
        {
            Debug.Log("Item is null");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // this is where you would check to see if you meet the item requirements if you want to create the warning for not being able to weald it in the ui

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;

    }
}
