using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IPowerUp
{
    [Header("--- Bomb Settings")]
    public float fuseTimer;
    public float blastRadius;
    public Vector2 bombPulseSpeed = new Vector2(1, 4);
    public LayerMask damageMask;
    [Space]
    public float launchForce;
    public Vector3 launchVector;
    public Vector3 deployOffset;
    [Space]
    public float destroyDelay;

    [Header("--- Visual References")]
    public ParticleSystem explosionParticles;
    public ParticleSystem fuseParticles;
    public GameObject mesh;

    [Header("--- Required References")]
    public Rigidbody rb;
    public Animator animator;

    private float eTime;

    private Transform shootPos;

    private void Awake()
    {
        this.transform.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deploy();
    }

    // Update is called once per frame
    void Update()
    {
        Handle();
    }

    public void Deploy()
    {
        Invoke("Destroy", fuseTimer);
        shootPos = transform;
        shootPos.Translate(deployOffset);
        shootPos.Rotate(launchVector);
        rb.AddForce(shootPos.forward * launchForce, ForceMode.Impulse);

        var systemShape = explosionParticles.shape;
        systemShape.radius = blastRadius;
    }

    public void Destroy()
    {
        Destroy(this.gameObject, destroyDelay);
        mesh.SetActive(false);
        explosionParticles.Play();
        fuseParticles.Stop();
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
        eTime += Time.deltaTime;
        float p = eTime / fuseTimer;

        animator.speed = Mathf.Lerp(bombPulseSpeed.x, bombPulseSpeed.y, p);
    }
}
