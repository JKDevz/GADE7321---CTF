using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Player Inventory Settings")]
    public FlagType pickupFlag;

    [Header(">>> Exposed for testing Only")]
    [SerializeField] private ItemType powerUpSlot;
    [SerializeField] private Flag flagSlot;

    private GameObject powerUpObject;

    #endregion

    #region INPUT ACTIONS

    public void OnFlagDrop(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            DropFlag();
        }
    }

    public void OnUseItem(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            UseItem();
        }
    }

    #endregion

    #region METHODS

    public void PickupItem(ItemType powerUp, GameObject prefab)
    {
        powerUpSlot = powerUp;
        powerUpObject = prefab;
    }

    public void PickupItem(Flag flagType)
    {
        flagSlot = flagType;
    }

    public void UseItem()
    {
        if (HasItem() && powerUpObject != null)
        {
            Instantiate(powerUpObject, gameObject.transform.position, Quaternion.identity, transform);
            powerUpObject = null;
            powerUpSlot = ItemType.Empty;
        }
    }

    public void DropFlag()
    {
        if (flagSlot != null)
        {
            flagSlot.DropFlag();
            flagSlot = null;
        }
    }

    #endregion

    #region UTIL METHODS

    public bool HasFlag()
    {
        if (flagSlot != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasItem()
    {
        if (powerUpSlot != ItemType.Empty && powerUpObject != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}

public enum ItemType
{
    Empty,
    Log,
    Bomb,
    Mine,
    Speed
}
