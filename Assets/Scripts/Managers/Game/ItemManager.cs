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
    public GameObject[] itemList;

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
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnRandomness.x, spawnRandomness.y));
            SpawnItem();
            yield return null;
        }
    }
    #endregion

    #region METHODS

    public void SpawnItem()
    {
        int i = 0;
        int counter = 0;

        do
        {
            i = Random.Range(0, itemSpawners.Length - 1);
            if (counter > itemSpawners.Length)
            {
                return;
            }
            else
            {
                counter++;
            }
        }
        while (itemSpawners[i].HasItem());

        itemSpawners[i].SpawnItem(itemList[Random.Range(0, itemList.Length - 1)]);

    }

    #endregion
}
