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
    [SerializeField] private GameObject itemPrefab;

    public delegate void OnItemPickup();
    public OnItemPickup onItemPickup;

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

    public void PickupItem(ItemType type, GameObject prefab)
    {
        powerUpSlot = type;
        itemPrefab = prefab;
        onItemPickup?.Invoke();
        //itemPrefab = new Item(item.itemType, item.itemPrefab, item.meshFilter, item.meshRenderer, item.animator, item.boxCollider, item.pickupOffset, item.pickupTime);
    }

    public void PickupItem(Flag flagType)
    {
        flagSlot = flagType;
    }

    public void UseItem()
    {
        if (HasItem() && itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, transform.rotation, this.transform);
            powerUpSlot = ItemType.Empty;
            itemPrefab = null;
        }
    }

    public void ClearItem()
    {
        if (HasItem() && itemPrefab != null)
        {
            powerUpSlot = ItemType.Empty;
            itemPrefab = null;
        }
    }

    public void DropFlag()
    {
        flagSlot.DropFlag();
        flagSlot = null;
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
        if (powerUpSlot != ItemType.Empty && itemPrefab != null)
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
