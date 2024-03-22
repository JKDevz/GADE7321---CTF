using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Flag Manager Settings")]
    public Flag redFlag;
    public Flag blueFlag;

    public bool playerHasFlag { get; protected set; }
    public bool aiHasFlag { get; protected set; }

    private static FlagManager _instance;

    #endregion

    #region ENABLES

    private void OnEnable()
    {
        GameManager.onRoundSetup += ResetFlags;
        Flag.onFlagPickup += OnFlagPickup;
        Flag.onFlagDrop += OnFlagDrop;
    }

    private void OnDisable()
    {
        GameManager.onRoundSetup -= ResetFlags;
        Flag.onFlagPickup -= OnFlagPickup;
        Flag.onFlagDrop -= OnFlagDrop;
    }

    #endregion

    #region SINGLETON STUFF

    public static FlagManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<FlagManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("FlagManager");
                    _instance = singleton.AddComponent<FlagManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion

    #region METHODS

    private void ResetFlags()
    {
        redFlag.ResetPosition();
        blueFlag.ResetPosition();
    }

    #endregion

    #region UTILITY METHODS

    private void OnFlagPickup(FlagType type)
    {
        if (type == FlagType.Blue)
        {
            playerHasFlag = true;
        }
        else if (type == FlagType.Red)
        {
            aiHasFlag = true;
        }
    }

    private void OnFlagDrop(FlagType type)
    {
        if (type == FlagType.Blue)
        {
            playerHasFlag = false;
        }
        else if (type == FlagType.Red)
        {
            aiHasFlag = false;
        }
    }

    #endregion
}
