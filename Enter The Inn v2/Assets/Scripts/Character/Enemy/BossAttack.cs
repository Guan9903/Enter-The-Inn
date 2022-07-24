using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage;

    [Range(1,10)]
    public int attacksNum;

    public float attackRate;

    protected GameObject instance;
    public Vector3 instancePos;

    public virtual void GetInstance()
    {
        if (instance == null)
        {
            instance = new GameObject();
            instance.transform.position = instancePos;
        }
    }

    public virtual void Initialize() { enabled = true; }

    public virtual void FixedUpdate() { }

    public virtual void Attack() { }
}
