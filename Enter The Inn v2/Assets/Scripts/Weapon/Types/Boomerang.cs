using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float rotateSpeed;

    private float floatDamage;
    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (activated)
            {
                transform.localEulerAngles += transform.up * rotateSpeed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var c = collision.gameObject;

        if (c.CompareTag("Enemy"))
        {
            c.GetComponent<EnemyManager>().GetHurt(floatDamage);
        }

        if (BoomerangWeapon.isFlying)
        {
            //FindObjectOfType<BoomerangWeapon>().WeaponReturn();
            BoomerangWeapon.isReturning = true;
            BoomerangWeapon.isFlying = false;

            //lastPos = weaponRig.position;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        activated = false;

        //if (!c.CompareTag("Player"))
        //{
        //}
            
    }

    public void GetDamageValue(float d)
    {
        floatDamage = d;
    }
}
