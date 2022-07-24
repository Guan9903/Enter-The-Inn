using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponHolder weaponHolder;

    public Text weaponName;
    public Text mSizeText;
    public Image weaponIcon;
    public Image cursor;
    public Image reload;
    public static bool reloading;

    private WeaponConfig config;
    private int mSize;
    private float rTime;
    private float cTime = 0f;

    private void Awake()
    {
        reloading = false;
        weaponHolder = FindObjectOfType<WeaponHolder>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetWeaponInfo();

        if (reloading)
        {
            if (cTime < rTime)
            {
                cTime += Time.deltaTime;

                reload.fillAmount = 1 - cTime / rTime;
            }
            else
            {
                reload.fillAmount = 0f;
                reloading = false;
                cTime = 0f;
            }
        }
    }

    void GetWeaponInfo()
    {
        if (weaponHolder.GetWeaponConfig() != null)
        {
            config = weaponHolder.GetWeaponConfig();
            weaponName.text = config.weaponName;
            weaponIcon.sprite = config.weaponIcon;
            weaponIcon.SetNativeSize();
            cursor.sprite = config.weaponCursor;
            rTime = config.reloadTime;
        }

        if (WeaponHolder.magazineSize < 0)
        {            
            mSizeText.text = null;
        }
        else
        {
            mSize = WeaponHolder.magazineSize;
            mSizeText.text = mSize.ToString();
        }
        
    }
}
