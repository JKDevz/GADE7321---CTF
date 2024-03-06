using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Flag References")]
    public FlagType flagType;
    public Transform flagSpawn;
    public Rigidbody rb;

    [Header("--- Flag Settings")]
    public FlagState flagState;
    public float flagDropLaunchStrength;
    public Vector3 flagHoldOffset = new Vector3(0, 2f, 0);

    public GameObject flagObject { get; private set; }
    private Transform flagHolder;

    #endregion

    #region DELEGATES

    public delegate void OnFlagPickup(FlagType flagType);
    public static OnFlagPickup onFlagPickup;

    public delegate void OnFlagDrop(FlagType flagType);
    public static OnFlagDrop onFlagDrop;

    public delegate void OnFlagReturned(FlagType flagType);
    public static OnFlagReturned onFlagReturned;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        flagObject = this.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collision");
            if (other.TryGetComponent(out PlayerInventory inv))
            {
                if (inv.pickupFlag == flagType)
                {
                    FlagPickedUp(ref inv);
                }
                else
                {
                    ResetPosition();
                }
            }
        }
    }

    private void Update()
    {
        if (flagHolder != null)
        {
            transform.position = flagHolder.position + flagHoldOffset;
        }
    }

    #endregion

    #region METHODS

    public void ResetPosition()
    {
        onFlagReturned?.Invoke(flagType);
        this.transform.position = flagSpawn.position;
        flagState = FlagState.Safe;
    }

    private void FlagPickedUp(ref PlayerInventory inv)
    {
        onFlagPickup?.Invoke(flagType);
        inv.PickupItem(this);
        flagHolder = inv.gameObject.transform;
        flagState = FlagState.Exposed;
    }

    public void DropFlag()
    {
        flagHolder = null;
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * flagDropLaunchStrength, ForceMode.Impulse);
    }

    #endregion
}

public enum FlagType
{
    None,
    Red,
    Blue
}

public enum FlagState
{
    Safe,
    Exposed
}
