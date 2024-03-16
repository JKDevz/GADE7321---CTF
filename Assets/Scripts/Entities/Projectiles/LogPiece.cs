using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPiece : MonoBehaviour
{
    [Header("--- Rolling Log Settings")]
    public float speed;
    [Space]
    public float lifetime;
    public float destroyLag;
    [Space]
    public LayerMask whatIsGround;

    [Header("--- Part references")]
    public GameObject visuals;
    public Collider boxCollider;
    public Collider triggerCollider;
    [Space]
    public ParticleSystem impactEffects;
    public ParticleSystem spawnEffects;

    [Header("--- Required References")]
    public Rigidbody rb;

    private bool isActive;

    void Start()
    {
        spawnEffects.Play();
        spawnEffects.Play();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(this.gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnImpact();
            if (other.TryGetComponent<Player>(out Player victim))
            {
                victim.Damage();
            }
        }
    }

    private void Update()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Force);

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        }
    }

    private void OnImpact()
    {
        DisableLog();
        impactEffects.Play();
        Destroy(this.gameObject, destroyLag);
    }

    private void DisableLog()
    {
        visuals.active = false;
        boxCollider.enabled = false;
        triggerCollider.enabled = false;
    }
}
