using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MeleeAttack : Action
{
    //public Animator animator;
    public SharedGameObject attackTarget;
    public SharedLayerMask attackLayers;
    public SharedInt damage;
    public SharedFloat attackRange;
    public SharedTransform attackPoint;

    private GameObject target;
    private LayerMask layers;
    private int damageValue;
    private float range;
    private Transform point;

    public override void OnStart()
    {
        target = attackTarget.Value;
        layers = attackLayers.Value;
        damageValue = damage.Value;
        range = attackRange.Value;
        point = attackPoint.Value;
    }

    public override TaskStatus OnUpdate()
    {
        Attack();
        //return TaskStatus.Running;
        return TaskStatus.Success;
    }

    void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(point.position, range, layers);

        for (int i = 0; i < hitPlayer.Length; i++)
        {
            //_ = hitPlayer[i];
            target.GetComponent<PlayerManager>().GetHurt(damageValue);
        }

    }

}
