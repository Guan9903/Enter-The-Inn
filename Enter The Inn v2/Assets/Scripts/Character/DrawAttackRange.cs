using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAttackRange : MonoBehaviour
{
    public float attackRange;
    public Transform attackPoint;

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
