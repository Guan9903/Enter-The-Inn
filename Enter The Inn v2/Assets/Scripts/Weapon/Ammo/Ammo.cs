using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public GameObject impactParticle;       //碰撞冲击粒子效果
    public GameObject projectileParticle;   //发射体粒子效果
    public GameObject muzzleParticle;       //枪口发射粒子效果

    [HideInInspector]
    public Vector3 impactNormal;    //用于旋转冲击粒子法线

    private float floatDamage;
    private int intDamage;
    private bool playerUse;
    private bool enemyUse;

    private void Awake()
    {
        //impactParticle = transform.Find("Impact Particle").gameObject;
        //projectileParticle = transform.Find("Projectile Particle").gameObject;
        //muzzleParticle = transform.Find("Muzzle Particle").gameObject;

        //impactParticle.SetActive(false);
        //projectileParticle.SetActive(false);
        //muzzleParticle.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //muzzleParticle.GetComponent<ParticleSystem>().Play();
        //muzzleParticle.transform.parent = null;
        //projectileParticle.GetComponent<ParticleSystem>().Play();

        //projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        //projectileParticle.GetComponent<ParticleSystem>().Play();
        //projectileParticle.transform.parent = transform;

        //muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        //muzzleParticle.GetComponent<ParticleSystem>().Play();

        //if (muzzleParticle)
        //{
        //    muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        //    //Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var c = collision.gameObject;

        if (playerUse)
        {
            if (!c.CompareTag("Player"))
            {
                if (c.CompareTag("Enemy"))
                {
                    c.GetComponent<EnemyManager>().GetHurt(floatDamage);
                    //Debug.Log(damage);
                }

                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //StartCoroutine(AmmoDelayedRecovery());

                AmmoPool.GetInstance().RecycleObj(gameObject);

                //impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                //impactParticle.GetComponent<ParticleSystem>().Play();
                //Destroy(projectileParticle, 3f);
                //Destroy(impactParticle, 5f);
            }
        }

        if (enemyUse)
        {
            if (!c.CompareTag("Enemy"))
            {
                if (c.CompareTag("Player"))
                {
                    c.GetComponent<PlayerManager>().GetHurt(intDamage);
                }

                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                AmmoPool.GetInstance().RecycleObj(gameObject);
                //StartCoroutine(AmmoDelayedRecovery());
            }
        }

    }

    IEnumerator AmmoDelayedRecovery()
    {
        yield return new WaitForSeconds(2f);
        AmmoPool.GetInstance().RecycleObj(gameObject);
    }

    public void GetUserLayerName(string user)
    {
        if (user == "Player")
        {
            playerUse = true;
        }
        if (user == "Enemy")
        {
            enemyUse = true;
        }
    }

    public void GetDamageValue(float d)
    {
        floatDamage = d;
    }

    public void GetDamageValue(int d)
    {
        intDamage = d;
    }

    private void OnEnable()
    {
        StartCoroutine(AutoRecycle());
    }

    IEnumerator AutoRecycle()
    {
        yield return new WaitForSeconds(6f);

        AmmoPool.GetInstance().RecycleObj(gameObject);
    }

}
