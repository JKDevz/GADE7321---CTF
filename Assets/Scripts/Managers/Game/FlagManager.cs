using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Flag Manager Settings")]
    public Flag redFlag;
    public Flag blueFlag;

    private static FlagManager _instance;

    #endregion

    #region ENABLES

    private void OnEnable()
    {
        GameManager.onRoundSetup += ResetFlags;
    }

    private void OnDisable()
    {
        GameManager.onRoundSetup -= ResetFlags;
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
}
