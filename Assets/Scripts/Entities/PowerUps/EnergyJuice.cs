using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyJuice : MonoBehaviour, IPowerUp
{
    [Header("--- Zibi Juice Settings")]
    public float duration;
    public float speedBoost;
    [Space]
    public float destroyDelay;

    [Header("--- Visual References")]
    public ParticleSystem ambientParticles;
    public ParticleSystem triggerParticles;

    private IEntity target;

    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.parent.TryGetComponent<Player>(out Player entity))
        {
            target = entity;
        }
        Deploy();
    }

    public void Deploy()
    {
        target.ModifySpeed(speedBoost);
        triggerParticles.Play();
        ambientParticles.Play();
        Invoke("Destroy", duration);
    }

    public void Destroy()
    {
        Destroy(this.gameObject, destroyDelay);
        target.ModifySpeed(-speedBoost);
        triggerParticles.Play();
        ambientParticles.Stop();
    }

    public void Handle()
    {

    }
}
