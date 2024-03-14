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
    [SerializeField] private PowerUp powerUpSlot;
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

    #endregion

    #region METHODS

    public void PickupItem(PowerUp powerUp, GameObject prefab)
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
        if (powerUpSlot != PowerUp.Empty && powerUpObject != null)
        {
            Instantiate(powerUpObject, gameObject.transform.position, Quaternion.identity, transform);
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
        if (powerUpSlot != PowerUp.Empty)
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

public enum PowerUp
{
    Empty,
    Log,
    Bomb,
    Mine,
    Speed
}
