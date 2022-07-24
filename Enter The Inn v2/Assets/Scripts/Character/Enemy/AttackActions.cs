using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AttackActions : Action
{
    public BossAttack attackType;
    public SharedGameObject attackTarget;

    public override void OnStart()
    {
        attackType.Initialize();
    }

    public override TaskStatus OnUpdate()
    {
        if (attackType.enabled == true)
        {
            attackType.FixedUpdate();
            return TaskStatus.Success;
        }
        else 
        {
            return TaskStatus.Failure;
        }
   
    }

}
