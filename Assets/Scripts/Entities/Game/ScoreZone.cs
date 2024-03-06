using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Score Zone Settings")]
    public FlagType checkFlag;

    private static ScoreZone _instance;

    #endregion

    #region DELEGATES

    public delegate void OnScore(FlagType type);
    public static OnScore onScore;

    #endregion

    #region COLLISIONS

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerInventory>(out PlayerInventory inv) && inv.pickupFlag == checkFlag)
            {
                onScore?.Invoke(checkFlag);
            }
        }
    }

    #endregion
}
