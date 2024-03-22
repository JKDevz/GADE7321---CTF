using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogPower : MonoBehaviour, IPowerUp
{
    [Header("--- Log Power Settings")]
    public float duration;
    public Vector3 deployOffset;
    [Space]
    public float destroyDelay;
    public LayerMask whatIsGround;

    [Header("--- Visual Effects")]
    public ParticleSystem triggerParticles;
    public GameObject visuals;

    [Header("--- Log Power References")]
    public NavMeshObstacle navMeshObstacle;

    #region UNITY METHODS

    private void Awake()
    {
        this.transform.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deploy();
        Invoke("Destroy", duration);
    }

    #endregion

    #region METHODS

    public void Deploy()
    {
        transform.Translate(deployOffset);
        triggerParticles.Play();

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, whatIsGround))
        {
            transform.position = hit.transform.position;
        }

        StartCoroutine(ToggleArmed(true));
        StartCoroutine(ToggleArmed(false, duration));
    }

    public void Destroy()
    {
        Destroy(this, destroyDelay);
        triggerParticles.Play();
        visuals.SetActive(false);
    }

    public void Handle()
    {
        
    }

    private IEnumerator ToggleArmed(bool b)
    {
        yield return new WaitForSeconds(0f);
        navMeshObstacle.enabled = b;
    }

    private IEnumerator ToggleArmed(bool b, float t)
    {
        yield return new WaitForSeconds(t);
        navMeshObstacle.enabled = b;
    }

    #endregion
}
