using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPower : MonoBehaviour, IPowerUp
{
    [Header("--- Log Power Settings")]
    [Range(1f, 10f)]
    public int logCount;
    [Range(1f, 5f)]
    public float rollWidth;
    public Vector3 offset;

    [Header("--- Log Power References")]
    public GameObject rollingLog;

    #region UNITY METHODS

    // Start is called before the first frame update
    void Start()
    {
        Deploy();
        Destroy();
    }

    #endregion

    #region METHODS

    public void Deploy()
    {
        Vector3 shootPos = transform.position + offset;

        Vector3 start = shootPos + Vector3.left * rollWidth/2;
        Vector3 end = shootPos + Vector3.right * rollWidth / 2;

        for (int i = 0; i < logCount; i++)
        {
            Vector3 spawnPos = Vector3.Lerp(start, end, i/logCount);
            Instantiate(rollingLog, spawnPos, Quaternion.identity, null);
        }
    }

    public void Destroy()
    {
        Destroy(this);
    }

    public void Handle()
    {
        
    }

    #endregion
}
