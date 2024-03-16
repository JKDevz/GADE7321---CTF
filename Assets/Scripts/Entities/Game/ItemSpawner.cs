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

    #region METHODS

    public void SpawnItem(GameObject item)
    {
        GameObject obj = Instantiate(item, gameObject.transform.position, Quaternion.identity, null);
        if (obj.TryGetComponent<Item>(out Item i))
        {
            this.item = i;
            hasItem = true;
        }
    }

    public bool HasItem()
    {
        return hasItem;
    }

    #endregion
}
