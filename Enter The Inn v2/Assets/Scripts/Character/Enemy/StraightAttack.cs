using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightAttack : BossAttack
{
    //public int damage;
    public GameObject aimTarget;
    public float aimTime;
    public float attackSpeed;

    private int num;
    private float rate;

    public override void GetInstance()
    {
        base.GetInstance();
        instance.name = "Straight Attack";
    }

    private void Awake()
    {
        GetInstance();
    }

    public override void Initialize()
    {
        base.Initialize();
        num = attacksNum;
        aimTarget = FindObjectOfType<PlayerManager>().gameObject;
        
    }

    public override void FixedUpdate()
    {
        instance.transform.position = new Vector3(transform.position.x, instancePos.y, transform.position.z);
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

            GameObject ammo = AmmoPool.GetInstance().GetObj("Ammo_Boss");

            ammo.GetComponent<Ammo>().GetDamageValue(damage);
            ammo.GetComponent<Ammo>().GetUserLayerName("Enemy");

            ammo.transform.parent = instance.transform;
            ammo.transform.position = instance.transform.position;
            ammo.transform.rotation = Quaternion.Euler(0, 0, 0);

            ammo.transform.LookAt(new Vector3(aimTarget.transform.position.x, ammo.transform.position.y, aimTarget.transform.position.z));
            ammo.GetComponent<Rigidbody>().velocity = ammo.transform.forward * attackSpeed;
        }
    }
}
