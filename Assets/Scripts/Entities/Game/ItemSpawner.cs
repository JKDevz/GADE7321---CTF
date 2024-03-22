using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Exposed for Testing")]
    public Vector3 spawnOffset;
    public Item item;
    public bool hasItem;

    #endregion

    private void OnEnable()
    {
        if (item != null)
        {
            item.onPickedUp += ItemTaken;
        }
    }

    private void OnDisable()
    {
        if (item != null)
        {
            item.onPickedUp -= ItemTaken;
        }
    }

    #region METHODS

    public void SpawnItem(GameObject item)
    {
        GameObject obj = Instantiate(item, gameObject.transform.position + spawnOffset, Quaternion.identity, null);
        if (obj.TryGetComponent<Item>(out Item i))
        {
            this.item = i;
            hasItem = true;
            this.item.onPickedUp += ItemTaken;
        }
    }

    private void ItemTaken()
    {
        this.item = null;
        hasItem = false;
    }

    public bool HasItem()
    {
        return hasItem;
    }

    #endregion
}
