using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    #region VARIABLES

    [Header("--- Required References")]
    public Player player;

    [Header("--- Attack Settings")]
    public float damage;
    public float impact;
    [Space]
    public LayerMask damageable;

    [Header("--- Item Settings")]
    public Vector3 castOffset = new Vector3(0f, 0f, 0f);
    [Space]
    public Action attackTiming;

    [Header("--- Range Settings")]
    public float meleeRange;
    public float itemRange;

    private float attackCooldown;

    #endregion

    #region INPUT ACTIONS

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Attack();
        }
    }

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
    }

    #endregion

    #region METHODS

    public void Attack()
    {
        if (!player.playerStats.isAttacking)
        {
            player.playerStats.isAttacking = true;//Stop the player from performing an attack
            StartCoroutine(DoAttack());
        }
    }

    public void UseItem()
    {

    }

    #endregion

    #region COROUTINES

    private IEnumerator DoAttack()
    {
        player.playerStats.isAttacking = true;//Stop the player from performing an attack

        yield return new WaitForSeconds(attackTiming.startLag);//Wait for the startup lag

        float attackDuration = Time.time + attackTiming.duration;

        do//Perform the attack
        {
            //Get all colliders that are entities.
            Collider[] hits = Physics.OverlapSphere(player.gameObject.transform.position, player.playerSettings.attackRange, damageable);
            if (hits != null)//As long as there are colliders
            {
                foreach(Collider hit in hits)//For every collider detected, try get their damage interface
                {
                    if (hit == player.boxCollider)//IF I have hit myself, ignore me
                    {
                        break;
                    }
                    else if (TryGetComponent(out IDamageable victim))
                    {
                        victim.Damage();//Damage the entity
                    }
                }
            }
            yield return null;
        }
        while (Time.time < attackDuration);//While the attack is still being performed, repeat.

        yield return new WaitForSeconds(attackTiming.endLag);//Wait for the end lag to finish

        player.playerStats.isAttacking = false;//Allow the player to perform another attack
    }

    #endregion

}

[System.Serializable]
public class Action
{
    public float startLag;//Windup before collision detection
    public float duration;//Duration in seconds where collision detection is possible
    public float endLag;//Wind-down before the attack can be performed again
}
