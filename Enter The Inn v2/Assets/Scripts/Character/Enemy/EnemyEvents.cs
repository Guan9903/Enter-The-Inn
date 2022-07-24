using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public int damage;
    public Transform attackPoint;
    public GameObject attackTarget;
    public float attackRange;

    public virtual void Attack() { }

    public virtual void CharacterDie()
    {
        Destroy(transform.parent.gameObject);
    }
}
