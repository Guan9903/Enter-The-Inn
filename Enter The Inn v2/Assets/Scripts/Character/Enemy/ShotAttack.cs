using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAttack : BossAttack
{
    public LayerMask ignoreLayers;
    public float ammoRange;
    public Transform attackPoint;

    private int num;
    private float rate;

    public override void Initialize()
    {
        base.Initialize();
        num = attacksNum;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (num > 0)
        {
            Attack();
        }
        else
        {
            enabled = false;
        }
    }

    public override void Attack()
    {
        rate -= Time.fixedDeltaTime;
        if (rate <= 0f)
        {
            rate = attackRate;
            num -= 1;

            RaycastHit hit;
            //layers = 1 << 15 | 1 << 13;    //15是Enemy所在层级，13是Ammo层级
            if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, ammoRange, ~ignoreLayers))
            {
                GameObject ammo = AmmoPool.GetInstance().GetObj("Arrow");

                ammo.GetComponent<Ammo>().GetDamageValue(damage);
                ammo.GetComponent<Ammo>().GetUserLayerName("Enemy");

                ammo.transform.position = attackPoint.position;
                ammo.transform.rotation = Quaternion.identity;

                ammo.transform.LookAt(hit.point);
                ammo.GetComponent<Rigidbody>().velocity = ammo.transform.forward * 15f;
                //Debug.Log(hit.transform.gameObject.name + " : " + hit.point);
            }
        } 
    }
}
