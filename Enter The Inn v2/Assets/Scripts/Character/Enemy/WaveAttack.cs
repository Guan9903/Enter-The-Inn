using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : BossAttack
{
    //public int damage;
    public float rotationAngle;
    public float waveSpeed;

    private int num;
    private float rate;
    private float selfRotation = 0f;

    public override void GetInstance()
    {
        base.GetInstance();
        instance.name = "Wave Attack";
    }

    private void Awake()
    {
        GetInstance();
    }

    public override void Initialize()
    {
        base.Initialize();
        num = attacksNum;

    }

    public override void FixedUpdate()
    {
        instance.transform.Rotate(Vector3.up * 60 * Time.fixedDeltaTime);
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
        Vector3 fireDirection = Vector3.forward;
        Quaternion startQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.up);

        rate -= Time.fixedDeltaTime;
        if (rate <= 0f)
        {
            rate = attackRate;
            num -= 1;
            for (int j = 0; j < (int)(360f / rotationAngle); j++)
            {
                GameObject ammo = AmmoPool.GetInstance().GetObj("Ammo_Boss");

                ammo.GetComponent<Ammo>().GetDamageValue(damage);
                ammo.GetComponent<Ammo>().GetUserLayerName("Enemy");

                var q = Quaternion.Euler(0, selfRotation, 0);

                ammo.transform.parent = instance.transform;
                ammo.transform.position = instance.transform.position;

                if (ammo.transform.rotation.y != 0f)
                {
                    ammo.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                ammo.transform.rotation *= q;

                ammo.GetComponent<Rigidbody>().velocity = fireDirection * waveSpeed;

                selfRotation += rotationAngle;
                selfRotation = selfRotation >= 360 ? selfRotation - 360 : selfRotation;
                fireDirection = startQuaternion * fireDirection;
            }
        }
    }
}
