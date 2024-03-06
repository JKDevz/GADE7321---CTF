using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES

    [Header("-- Movement Settings Reference")]
    [SerializeField] protected PlayerSettingsSO playerSettings;
    [SerializeField] protected NavMeshAgent agent;

    //Movement Stats
    protected float speed;

    //Input Variables
    private Vector2 moveInput;

    protected bool isSprintHeld;
    protected bool isCarryingFlag;

    private float sprintModifier;
    private float flagCarryModifier;

    private float sprintApplier = 1f;
    private float flagPenaltyApplier = 0.9f;

    #endregion

    #region EVENTS

    public delegate void OnTrigger();
    public static event OnTrigger onTrigger;

    public delegate void OnMoving();
    public static event OnMoving onMoving;

    public delegate void OnSprinting();
    public static event OnSprinting onSprinting;

    #endregion

    #region MAIN LOOP

    private void Awake()
    {
        LoadValues();
    }

    private void Update()
    {
        TryMove();
    }

    #endregion

    #region PLAYER ACTIONS

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isSprintHeld = true;
            sprintApplier = sprintModifier;
        }
        else
        {
            isSprintHeld = false;
            sprintApplier = 1f;
        }
    }

    #endregion

    #region METHODS

    public void TryMove()
    {
        if (moveInput.magnitude != 0)//Moves the player when player gives horizontal input
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            float moveSpeed = speed;

            if (isSprintHeld)
            {
                moveSpeed += sprintApplier * speed;
            }

            if (isCarryingFlag)
            {
                moveSpeed -= flagCarryModifier * moveSpeed;
            }

            agent.velocity = move * moveSpeed;
        }
    }

    public void LoadValues()
    {
        speed = playerSettings.speed;
        sprintModifier = playerSettings.sprintModifier;
        flagCarryModifier = playerSettings.flagCarryModifier;

        agent.angularSpeed = playerSettings.angularSpeed;
        agent.autoBraking = playerSettings.autoBraking;
        agent.acceleration = playerSettings.acceleration;

        agent.radius = playerSettings.radius;
        agent.height = playerSettings.height;
    }

    #endregion
}
