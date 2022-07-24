using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoomerangWeapon : IWeapon
{
    
    public Rigidbody weaponRig;
    //public Transform curvePoint;
    public float force;
    public float returnSpeed;

    [Range(0.3f, 1f)]
    public float maxFlyTime;
    public static bool isReturning = false;
    public static bool isFlying = false;

    public Animator anim;               // 武器动画
    
    private float flyTime;
    
    //private Vector3 lastPos;
    //private float dis;
    //private float time = 0f;

    private void Awake()
    {
        IWeaponSet();
        weaponRig.GetComponent<Boomerang>().GetDamageValue(weaponDamage);
    }

    // Start is called before the first frame update
    void Start()
    { 
        ResetWeapon();
        //weaponUI.SetActive(true);
        //weaponRig.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetButtonDown("Fire1") && !isReturning && !isFlying)
            {
                Attack();
            }

        }
    }

    private void LateUpdate()
    {
        if (Time.timeScale != 0)
        {
            if (isFlying)
            {            
                flyTime -= Time.deltaTime;
                if (flyTime <= 0f)
                {
                    WeaponReturn();
                }
            }

            if (isReturning)
            {
                weaponRig.position = Vector3.MoveTowards(weaponRig.position, muzzleTrans.position, Time.deltaTime * returnSpeed);
                
                if (Vector3.Distance(weaponRig.position, muzzleTrans.position) <= 1f)
                {
                    ResetWeapon();
                }
            }

            WeaponInfoUpdate();
        }
    }

    public override void Attack()
    {
        weaponUI.SetActive(false);
        weaponRig.gameObject.SetActive(true);

        anim.Play("attack");
        source.clip = attackAudio;
        source.Play();

        isFlying = true;
        flyTime = maxFlyTime;
        weaponRig.isKinematic = false;
        weaponRig.transform.SetParent(null);
        weaponRig.AddForce(muzzleTrans.forward * force, ForceMode.Impulse);

        WeaponHolder.enableUse = false;
    }

    public void WeaponReturn()
    {
        isReturning = true;
        isFlying = false;

        weaponRig.velocity = Vector3.zero;
        weaponRig.isKinematic = true;
    }

    void ResetWeapon()
    {   
        weaponRig.transform.SetParent(transform);
        weaponRig.transform.localPosition = muzzleTrans.localPosition;
        weaponRig.transform.localRotation = muzzleTrans.localRotation;

        weaponUI.SetActive(true);
        weaponRig.gameObject.SetActive(false);
        //time = 0f;
        isReturning = false;
        isFlying = false;
        WeaponHolder.enableUse = true;
    }

    public override void WeaponInfoUpdate()
    {
        base.WeaponInfoUpdate();
        WeaponHolder.magazineSize = -1;
    }

    /// <summary>
    /// 二次贝塞尔曲线
    /// </summary>
    /// <param name="t"> 取值范围[0, 1] </param>
    /// <param name="p0"> 武器上一次所处位置 </param>
    /// <param name="p1"> 中间点 </param>
    /// <param name="p2"> 目标点 </param>
    /// <returns></returns>
    Vector3 GetQuadraticBezierCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

    Vector3 GetLinearBezierCurvePoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;
        Vector3 p = u * p0 + t * p1;
        return p;
    }
}
