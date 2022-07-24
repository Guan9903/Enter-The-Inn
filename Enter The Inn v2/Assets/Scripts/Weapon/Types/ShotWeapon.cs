using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWeapon : IWeapon
{
    //public WeaponHolder weaponHolder;
    public LayerMask ignoreLayer;       // 射线忽略层级
    public Animator anim;               // 武器动画

    private int mSize;
    private float rTime;
    private float fRate;

    //private bool firstFire = true;
    //private float next2Fire;

    private bool reloading = false;

    private string ammoName;
    

    private void Awake()
    {
        IWeaponSet();
        mSize = magazineSize;
        rTime = reloadTime;
        fRate = fireRate;
        ammoName = ammoObj.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (!reloading)
            {
                if (mSize > 0)
                {
                    fRate -= Time.deltaTime;
                    if (Input.GetButton("Fire1") && fRate <= 0f)
                    {
                        anim.Play("attack");
                        source.clip = attackAudio;
                        source.Play();
                        Attack();
                        fRate = fireRate;
                        mSize--;
                    }
                }
                if (mSize <= 0 && Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ReloadAmmo());
                }

                if (Input.GetKeyDown(KeyCode.R))    // 装填子弹
                {
                    //FindObjectOfType<AudioManager>().PlayEffect("sound_reload");
                    StartCoroutine(ReloadAmmo());
                }
            }

            
            WeaponInfoUpdate();
        }
        
    }


    IEnumerator ReloadAmmo()
    {
        reloading = true;
        source.clip = reloadAudio;
        source.Play();
        WeaponUI.reloading = true;

        yield return new WaitForSeconds(rTime);

        mSize = magazineSize;
        reloading = false;
    }

    public override void Attack()
    {
        Ray ray = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        RaycastHit hit;
        Vector3 hitPoint;
        //int closeLayerMask = 1 << 12 | 1 << 13 | 1 << 9;    // 12是Player所在层级，13是Ammo层级
        int closeLayerMask = ignoreLayer;
        if (Physics.Raycast(ray, out hit, fireRange, ~closeLayerMask))
        {
            hitPoint = hit.point;
            AmmoSpawn(hitPoint);
        }
        else
        {
            //Vector3 newPos = fpsCam.transform.forward * fireRange;
            hitPoint = ray.GetPoint(fireRange);
            AmmoSpawn(hitPoint);
        }
        //Debug.Log(hit.transform.gameObject.name + " : " + hit.point);
    }

    void AmmoSpawn(Vector3 hitPoint)
    {
        GameObject ammo = AmmoPool.GetInstance().GetObj(ammoName);

        ammo.GetComponent<Ammo>().GetDamageValue(weaponDamage);
        ammo.GetComponent<Ammo>().GetUserLayerName("Player");

        ammo.transform.position = muzzleTrans.position;
        ammo.transform.rotation = Quaternion.identity;
        //GameObject ammo = Instantiate(ammoObj, muzzleTrans.position, Quaternion.identity) as GameObject;
        ammo.transform.LookAt(hitPoint);
        //ammo.GetComponent<Rigidbody>().AddForce(ammo.transform.forward * ammoSpeed *100);
        ammo.GetComponent<Rigidbody>().velocity = ammo.transform.forward * ammoSpeed;
        
    }

    public override void WeaponInfoUpdate()
    {
        base.WeaponInfoUpdate();
        WeaponHolder.magazineSize = mSize;
        WeaponHolder.reloadTime = rTime;
    }
}
