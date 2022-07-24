using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEvent : EnemyEvents
{
    public LayerMask attackLayers;

    public override void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, attackLayers);

        for (int i = 0; i < hitPlayer.Length; i++)
        {
            hitPlayer[i].GetComponent<PlayerManager>().GetHurt(damage);
        }
    }
}
