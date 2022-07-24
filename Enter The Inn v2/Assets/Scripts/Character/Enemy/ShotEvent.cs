using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEvent : EnemyEvents
{
    public LayerMask ignoreLayers;
    public float ammoSpeed;

    public override void Attack()
    {
        RaycastHit hit;
        //layers = 1 << 15 | 1 << 13;    //15是Enemy所在层级，13是Ammo层级
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, attackRange, ~ignoreLayers))
        {
            GameObject ammo = AmmoPool.GetInstance().GetObj("Arrow");

            ammo.GetComponent<Ammo>().GetDamageValue(damage);
            ammo.GetComponent<Ammo>().GetUserLayerName("Enemy");

            ammo.transform.position = attackPoint.position;
            ammo.transform.rotation = Quaternion.identity;

            ammo.transform.LookAt(hit.point);
            ammo.GetComponent<Rigidbody>().velocity = ammo.transform.forward * ammoSpeed;
            //Debug.Log(hit.transform.gameObject.name + " : " + hit.point);
        }
    }
}
