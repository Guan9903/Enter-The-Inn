using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShotWeapon : IWeapon
{
    private string ammoName;

    private void Awake()
    {
        IWeaponSet();
        ammoName = ammoObj.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0f)
        {
            HookShoot();
            WeaponInfoUpdate();
        }
    }

    void HookShoot()
    {
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
    }

    public override void Attack()
    {
        RaycastHit hit;
        int closeLayerMask = 1 << 12 | 1 << 13 | 1 << 9;    //12是Player所在层级，13是Ammo层级
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, fireRange, ~closeLayerMask))
        {
            if (hit.transform == null)
            {
                Vector3 newPos = fpsCam.transform.forward * fireRange;
                hit.transform.position = newPos;
            }



            GameObject ammo = AmmoPool.GetInstance().GetObj(ammoName);

            ammo.GetComponent<Ammo>().GetDamageValue(weaponDamage);
            ammo.GetComponent<Ammo>().GetUserLayerName("Player");

            ammo.transform.position = muzzleTrans.position;
            ammo.transform.rotation = Quaternion.identity;

            ammo.transform.LookAt(hit.point);
            //ammo.GetComponent<Rigidbody>().AddForce(ammo.transform.forward * ammoSpeed *100);
            ammo.GetComponent<Rigidbody>().velocity = ammo.transform.forward * ammoSpeed;
            //Debug.Log(hit.transform.gameObject.name + " : " + hit.point);
        }
    }

    public override void WeaponInfoUpdate()
    {
        base.WeaponInfoUpdate();
    }
}
