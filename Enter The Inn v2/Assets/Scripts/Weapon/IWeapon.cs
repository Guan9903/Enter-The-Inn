using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IWeapon : MonoBehaviour
{
    public WeaponConfig config;   
    public Transform muzzleTrans;
    public AudioClip attackAudio;
    public AudioClip reloadAudio;
    
    public GameObject weaponUI;
    protected AudioSource source;
    protected Camera fpsCam;
    protected int id;
    protected string weaponName;
    protected Sprite weaponIcon;
    protected Sprite weaponCursor;
    protected float weaponDamage;      // 武器伤害

    protected GameObject ammoObj;      // 弹药预制体
    protected Sprite ammoIcon;
    protected int ammoNums;            // 弹药数量
    protected int magazineSize;        // 弹夹容量
    protected float reloadTime;        // 换弹时间
    protected float ammoSpeed;         // 弹药速度
    protected float fireRange;         // 射击距离
    protected float fireRate;          // 射击频率

    protected float weaponDurability;  // 武器耐久（近战以及之后的法术攻击）

    public void IWeaponSet()
    {
        id = config.id;
        weaponName = config.weaponName;
        weaponDamage = config.weaponDamage;
        ammoObj = config.ammoObj;
        ammoNums = config.ammoNums;
        magazineSize = config.magazineSize;
        reloadTime = config.reloadTime;
        ammoSpeed = config.ammoSpeed;
        fireRange = config.fireRange;
        fireRate = config.fireRate;

        source = GetComponent<AudioSource>();
        fpsCam = FindObjectOfType<PlayerManager>().transform.Find("Main Camera").GetComponent<Camera>();
        
    }

    public virtual void Attack() { }

    public virtual void WeaponInfoUpdate()
    {
        WeaponHolder.selectedWeaponName = weaponName;
    }

}
