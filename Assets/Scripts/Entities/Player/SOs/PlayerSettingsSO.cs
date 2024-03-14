using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Settings", menuName = "ScriptableObjects/Settings/Entities/Player", order = 0)]
public class PlayerSettingsSO : ScriptableObject
{
    [Header("-- Player Movement Settings")]
    public float speed;
    public float sprintModifier;
    public float flagCarryModifier;
    public float acceleration;
    [Space]
    public float attackRange;
    [Space]
    public float stamina;
    public float staminaTickRegen;
    public float staminaTickDelay;
    public float staminaTickCost;

    [Header("-- Player Attack Settings")]
    public float stunDuration;
    public float meleeRange;
    public LayerMask canDamage;

    [Header("-- Player Navmesh Settings")]
    public float angularSpeed;
    public float stoppingDistance;
    public bool autoBraking;

    [Header("-- Navmesh Obstacle Avoidance")]
    public float radius;
    public float height;

    [Header("-- AI Settings")]
    public float baseDangerRadius;
}
