using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEvent : EnemyEvents
{
    [Range(1, 10)]
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
            instance.name = "Wave Attack";
        }
    }

    public override void Attack()
    {
        
    }
}
