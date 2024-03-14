using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Animation References")]
    public Animator animator;
    public Player player;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        this.player.playerStats = new PlayerStats();
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
    }

    private void Update()
    {
        animator.SetBool("isWalking", player.playerStats.isTryWalk);
        animator.SetBool("isSprinting", player.playerStats.isSprinting);
        animator.SetBool("isAttacking", player.playerStats.isAttacking);
    }

    #endregion
}
