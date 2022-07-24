using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickDrop : MonoBehaviour
{  
    public float pickUpRange;           // 拾取范围
    public GameObject pressEPrefab;     // 按E键拾取预制体
    public GameObject weaponIcon;
    public AudioClip pickAudio;

    private bool equipped;              // 武器是否装备
    private Transform player;
    private Transform weaponHolder;
    private AudioSource source;


    void Awake()
    {
        player = FindObjectOfType<PlayerManager>().transform;
        weaponHolder = FindObjectOfType<WeaponHolder>().transform;
        pressEPrefab.SetActive(false);
        weaponIcon.SetActive(true);

        transform.GetComponent<IWeapon>().enabled = false;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Vector3.Distance(transform.position, player.position) <= pickUpRange && equipped == false)
            {
                pressEPrefab.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    transform.SetParent(weaponHolder);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                    transform.localScale = Vector3.one;
                    equipped = true;
                    pressEPrefab.SetActive(false);
                    weaponIcon.SetActive(false);

                    FindObjectOfType<AudioManager>().PlayEffect("sound_pick");
                   
                    //source.clip = pickAudio;
                    //source.Play();

                    if (GetComponent<LookAtTarget>() != null)
                    {
                        GetComponent<LookAtTarget>().enabled = false;
                    }
                }
            }
            else
            {
                pressEPrefab.SetActive(false);
            }

            if (transform.GetComponent<IWeapon>().enabled == false && equipped == true)
            {
                transform.GetComponent<IWeapon>().enabled = true;
                if (weaponHolder.childCount > 1)
                {
                    gameObject.SetActive(false);
                }
            }           
        }
    }



}
