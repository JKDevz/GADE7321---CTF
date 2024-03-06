using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barricade : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Barricade References")]
    public Collider collider;
    public NavMeshObstacle navMeshObstacle;
    public MeshRenderer meshRenderer;

    private bool barricadeActive = true;

    #endregion

    #region ENABLES

    private void OnEnable()
    {
        GameManager.onRoundSetup += EnableBarricade;
        GameManager.onRoundPlaying += DropBarricade;
    }

    private void OnDisable()
    {
        GameManager.onRoundSetup -= EnableBarricade;
        GameManager.onRoundPlaying -= DropBarricade;
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (this.TryGetComponent<Collider>(out Collider cmp))
        {
            collider = cmp;
        }
        if (this.TryGetComponent<NavMeshObstacle>(out NavMeshObstacle obs))
        {
            navMeshObstacle = obs;
        }
    }

    #endregion

    #region METHODS

    private void EnableBarricade()
    {
        Debug.Log("Enabling Barricade");
        GetComponent<Collider>().enabled = true;
        navMeshObstacle.enabled = true;
        meshRenderer.enabled = true;
        collider.enabled = true;
        barricadeActive = true;

        GameManager.onRoundSetup -= EnableBarricade;
        GameManager.onRoundPlaying += DropBarricade;
    }

    private void DropBarricade()
    {
        Debug.Log("Disabling Barricade");
        GetComponent<Collider>().enabled = false;
        navMeshObstacle.enabled = false;
        meshRenderer.enabled = false;
        collider.enabled = false;
        barricadeActive = false;

        GameManager.onRoundPlaying -= DropBarricade;
        GameManager.onRoundSetup += EnableBarricade;
    }

    #endregion
}
