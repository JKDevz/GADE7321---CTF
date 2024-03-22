using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IEntity
{
    [Header("--- Player References")]
    public PlayerSettingsSO playerSettings;
    public PlayerInventory Inventory;
    public PlayerMovement PlayerMovement;
    public PlayerAttacking Attacking;
    public PlayerAnimator Animator;
    public PlayerStats playerStats;
    public NavMeshAgent agent;

    public Collider boxCollider;

    #region ENABLE/DISABLE

    private void OnEnable()
    {
        GameManager.onRoundFinished += OnRoundFinished;
    }

    private void OnDisable()
    {
        GameManager.onRoundFinished -= OnRoundFinished;
    }

    #endregion

    private void Awake()
    {
        LoadValues();
    }

    public void Damage(GameObject attacker)
    {
        onDamage?.Invoke(attacker);
    }

    public void ModifySpeed(float modifyAmount)
    {
        playerStats.speedModifier += modifyAmount;
    }

    public void OnRoundFinished()
    {
        Inventory.DropFlag();
    }

    public delegate void OnDamage(GameObject attacker);
    public OnDamage onDamage;

    public void LoadValues()
    {
        playerStats = new PlayerStats();

        playerStats.speed = playerSettings.speed;
        playerStats.flagCarryModifier = playerSettings.flagCarryModifier;
        playerStats.knockbackDuration = playerSettings.knockbackDuration;
        playerStats.knockbackStrength = playerSettings.knockbackStrength;
        playerStats.speedModifier = 0f;

        playerStats.stunDuration = playerSettings.stunDuration;
        playerStats.meleeRange = playerSettings.meleeRange;

        agent.angularSpeed = playerSettings.angularSpeed;
        agent.autoBraking = playerSettings.autoBraking;
        agent.acceleration = playerSettings.acceleration;
        agent.radius = playerSettings.radius;
        agent.height = playerSettings.height;

        playerStats.radius = playerSettings.radius;
        playerStats.height = playerSettings.height;
    }
}

public interface IEntity
{
    public void Damage(GameObject attacker);
    public void ModifySpeed(float modifyAmount);
}



[System.Serializable]
public class PlayerStats
{
    public bool isSprinting;
    public bool isAttacking;
    public bool isUsingItem;
    public bool isTryWalk;
    public bool isStunned;

    public float knockbackDuration;
    public float knockbackStrength;

    public float speed;
    public float acceleration;
    public float speedModifier;

    public float sprintModifier;
    public float flagCarryModifier;

    public float sprintCooldown;
    public float sprintTimer;

    public float stunDuration;
    public float meleeRange;

    public float stunTimer;

    public float radius;
    public float height;
}

public enum Stamina
{
    Drain,
    Regen
}
