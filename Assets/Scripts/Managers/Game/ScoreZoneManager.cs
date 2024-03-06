using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZoneManager : MonoBehaviour
{
    #region VARIABLES

    [Header("--- ScoreZone References")]
    public ScoreZone blueScoreZone;
    public ScoreZone redScoreZone;

    private static ScoreZoneManager _instance;

    #endregion

    #region SINGLETON

    public static ScoreZoneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ScoreZoneManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("ScoreZoneManager");
                    _instance = singleton.AddComponent<ScoreZoneManager>();
                }
            }
            return _instance;
        }
        private set { }
    }

    #endregion
}
