using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TYPES
{
    Automatic,      // 全自动
    Semiautomatic,  // 半自动
    Melee,          // 近战
    //Beam,           // 光束
    //Charged,        // 充能 
}

[CreateAssetMenu]
public class WeaponConfig : ScriptableObject
{
    public TYPES weaponType;        // 武器类型

    public int id;
    public string weaponName = "New Weapon";
    public Sprite weaponIcon;
    public Sprite weaponCursor;
    public float weaponDamage;      // 武器伤害

    public GameObject ammoObj;      // 弹药预制体
    public Sprite ammoIcon;
    public int ammoNums;            // 弹药数量
    public int magazineSize;        // 弹夹容量
    public float reloadTime;        // 换弹时间
    public float ammoSpeed;         // 弹药速度
    public float fireRange;         // 射击距离
    public float fireRate;          // 射击频率

    public float weaponDurability;  // 武器耐久（近战以及之后的法术攻击）

}
