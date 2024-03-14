using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Spawner Settings")]
    public Vector2 spawnRandomness;
    [Space]
    public ItemSpawner[] itemSpawners;

    private float spawnTimer;

    private static ItemManager _instance;

    #endregion

    #region SINGLETON STUFF

    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ItemManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("ItemManager");
                    _instance = singleton.AddComponent<ItemManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion

    #region MAIN LOOP

    private IEnumerator Start()
    {
        yield return null;
    }
    #endregion

    #region METHODS

    public void SpawnItem()
    {

    }

    #endregion
}
