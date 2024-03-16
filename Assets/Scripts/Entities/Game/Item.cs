using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("--- Item Settings")]
    public ItemType itemType;
    public GameObject itemPrefab;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Animator animator;
    public Collider boxCollider;

    [Header("--- Pickup Settings")]
    public Vector3 pickupOffset;
    public float pickupTime = 0.4f;

    public delegate void OnPickedUp();
    public OnPickedUp onPickedUp;

    public Item(Mesh mesh, ItemType type)
    {
        itemType = type;
        meshFilter.mesh = mesh;
    }

    private void OnDisable()
    {
        onPickedUp?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this, pickupTime);//Destroy this after the set time
            GiveItem(other);//Give the item to the player
            OnPickup(other);//Play the pickup animations
        }
    }

    private void OnPickup(Collider eventCollider)
    {
        onPickedUp?.Invoke();
        animator.SetTrigger("onPickup");
        transform.SetParent(eventCollider.transform, false);
        transform.localPosition = pickupOffset;
        boxCollider.enabled = false;
    }

    private void GiveItem(Collider eventCollider)
    {
        if (eventCollider.TryGetComponent<PlayerInventory>(out PlayerInventory inv))
        {
            inv.PickupItem(itemType, itemPrefab);
        }
    }

    public void Disable()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }
}
