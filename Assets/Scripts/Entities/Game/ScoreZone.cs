using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Score Zone Settings")]
    public FlagType checkFlag;

    #endregion

    #region COLLISIONS

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerInventory>(out PlayerInventory inv))
            {
                if (inv.pickupFlag == checkFlag && inv.HasFlag())
                {
                    inv.DropFlag();
                    ScoreZoneManager.onScore?.Invoke(checkFlag);
                }
            }
        }
    }

    #endregion
}
