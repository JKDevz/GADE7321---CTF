using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Player References")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject AI;

    [Header("--- Player Settings")]
    public Transform playerSpawn;
    public Transform aiSpawn;

    private static PlayerManager _instance;

    #endregion

    #region SINGLETON STUFF

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("PlayerManager");
                    _instance = singleton.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion

    #region ENABLES

    private void OnEnable()
    {
        GameManager.onRoundSetup += RespawnPlayers;
    }

    private void OnDisable()
    {
        GameManager.onRoundSetup -= RespawnPlayers;
    }

    #endregion

    #region METHODS

    public void RespawnPlayers()
    {
        Player.transform.position = playerSpawn.position;
        AI.transform.position = aiSpawn.position;
    }

    public GameObject GetPlayer()
    {
        return Player;
    }

    public GameObject GetAI()
    {
        return AI;
    }

    #endregion
}
