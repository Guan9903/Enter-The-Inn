using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Range(1,4)]
    public int maxAmount = 4;
    public int selectedWeapon = 0;
    public static bool enableUse;
    public static string selectedWeaponName;
    public static int magazineSize;
    public static int ammoNums;
    public static float reloadTime;

    private bool noWeapon = true;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        enableUse = true;
        noWeapon = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && enableUse)
        {
            int preSelected = selectedWeapon;

            if (!WeaponUI.reloading)
            {
                if (transform.childCount == 0)
                {
                    noWeapon = true;
                }
                else
                {
                    noWeapon = false;
                    WeaponChange(preSelected);
                }              
            }
        }       
    }

    void WeaponChange(int preSelected)
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeapon = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeapon = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            selectedWeapon = 3;

        if (preSelected != selectedWeapon)
            SelectWeapon();

    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    public WeaponConfig GetWeaponConfig()
    {
        if (noWeapon)
        {
            return null;
        }
        else
            return transform.GetChild(selectedWeapon).GetComponent<IWeapon>().config;
    }
}
