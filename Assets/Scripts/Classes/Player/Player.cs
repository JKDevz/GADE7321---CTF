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

    public float stunTimer;

    public float stamina;
    public float staminaTickRegen;
    public float staminaTickDelay;
    public float staminaTickCost;

    private float maxStamina;

    private bool staminaDraining;
    private float staminaTick;

    public void DrainStamina()
    {
        staminaDraining = true;
        if (Time.time > staminaTick)
        {
            if (stamina > 0)
            {
                stamina -= staminaTickCost;
            }
            else
            {
                stamina = maxStamina;
            }

            stamina = Time.time + staminaTickDelay;
        }
    }

    public void RegenStamina()
    {
        staminaDraining = false;
        if (Time.time > staminaTick)
        {
            if (stamina <= maxStamina)
            {
                stamina += staminaTickRegen;
            }
            else
            {
                stamina = 0;
            }

            stamina = Time.time + staminaTickDelay;
        }
    }
}

public enum Stamina
{
    Drain,
    Regen
}
