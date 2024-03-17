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
    [SerializeField] private Item itemRef;

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

    public void PickupItem(ItemType type, Item item)
    {
        powerUpSlot = type;
        itemRef = new Item(item.itemType, item.itemPrefab, item.meshFilter, item.meshRenderer, item.animator, item.boxCollider, item.pickupOffset, item.pickupTime);
    }

    public void PickupItem(Flag flagType)
    {
        flagSlot = flagType;
    }

    public void UseItem()
    {
        if (HasItem() && itemRef != null)
        {
            Instantiate(itemRef.itemPrefab, gameObject.transform.position, Quaternion.identity, transform);
            powerUpSlot = ItemType.Empty;
            itemRef = null;
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
        if (powerUpSlot != ItemType.Empty && itemRef != null)
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
