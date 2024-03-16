using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("--- Player References")]
    public PlayerSettingsSO playerSettings;
    public PlayerInventory Inventory;
    public PlayerMovement PlayerMovement;
    public PlayerAttacking Attacking;
    public PlayerAnimator Animator;
    public PlayerStats playerStats;

    public Collider boxCollider;

    public void Damage()
    {
        onDamage?.Invoke();
    }

    public delegate void OnDamage();
    public OnDamage onDamage;
}

public interface IDamageable
{
    public void Damage();
}

[System.Serializable]
public class PlayerStats
{
    public bool isSprinting;
    public bool isAttacking;
    public bool isUsingItem;
    public bool isTryWalk;
    public bool isStunned;

    public float speed;
    public float acceleration;

    public float sprintModifier;
    public float flagCarryModifier;

    public float sprintCooldown;
    public float sprintTimer;

    public float stunDuration;
    public float meleeRange;

    public float stunTimer;
}

public enum Stamina
{
    Drain,
    Regen
}
