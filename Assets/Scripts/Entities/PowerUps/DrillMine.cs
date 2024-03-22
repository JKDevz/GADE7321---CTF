using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrillMine : MonoBehaviour, IPowerUp
{
    [Header("--- Drill Mine Settings")]
    public float deployTime = 1f;
    public float armingTime = 2f;
    public float detectionRadius = 1.5f;
    public float blastRadius = 7f;
    public LayerMask damageMask;
    [Space]
    public Color armedCol;
    public Color unarmedCol;
    [Space]
    public float destroyDelay;
    public LayerMask whatIsGround;

    [Header("--- Visual references")]
    public ParticleSystem armedParticles;
    public ParticleSystem explosionParticles;
    public GameObject mesh;
    public Material coreMat;

    [Header("--- Required references")]
    public SphereCollider triggerArea;
    public NavMeshObstacle navMeshObstacle;

    private void Awake()
    {
        this.transform.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deploy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy();
        }
    }

    public void Deploy()
    {
        triggerArea.radius = detectionRadius;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f, whatIsGround))
        {
            transform.position = hit.transform.position;
        }

        this.StartCoroutine(ToggleArmed(false));
        this.StartCoroutine(ToggleArmed(true, deployTime + armingTime));
    }

    public void Destroy()
    {
        Destroy(this.gameObject, destroyDelay);
        triggerArea.enabled = false;
        mesh.SetActive(false);
        explosionParticles.Play();
        armedParticles.Stop();
        //Get all colliders that are entities.
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, blastRadius, damageMask);
        if (hits != null)//As long as there are colliders
        {
            foreach (Collider hit in hits)//For every collider detected, try get their damage interface
            {
                if (hit.TryGetComponent(out IEntity victim))
                {
                    victim.Damage(this.gameObject);//Damage the entity
                }
            }
        }
    }

    public void Handle()
    {

    }

    private IEnumerator ToggleArmed(bool b)
    {
        yield return new WaitForSeconds(0f);
        triggerArea.enabled = b;
        navMeshObstacle.enabled = b;

        if (b == true)
        {
            coreMat.color = armedCol;
            armedParticles.Play();
        }
        else
        {
            coreMat.color = unarmedCol;
            armedParticles.Stop();
        }
    }

    private IEnumerator ToggleArmed(bool b, float t)
    {
        yield return new WaitForSeconds(t);
        triggerArea.enabled = b;
        navMeshObstacle.enabled = b;

        if (b == true)
        {
            coreMat.color = armedCol;
        }
        else
        {
            coreMat.color = unarmedCol;
        }
    }
}
