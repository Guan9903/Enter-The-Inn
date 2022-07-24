using UnityEngine;

public class EnemyManager : ICharacter
{
    public float enemyHealth;
    //public Animator animator;

    private Material matWhite;
    private Material matDefault;

    private void Awake()
    {
        ICharacterSet();
        enemyHealth = health;
    }

    // Start is called before the first frame update
    void Start()
    {
        matWhite = Resources.Load("Materials/White Flash", typeof(Material)) as Material;
        matDefault = enemySR.material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void GetHurt(float floatDamage)
    { 
        enemyHealth -= floatDamage;
        enemySR.material = matWhite;

        if (enemyHealth <= 0f)
        {
            if (source.isPlaying && source.clip == dieAudio)
                return;
            else
            {
                source.clip = dieAudio;
                source.Play();
            }

            //animator.SetTrigger("die");
            //Destroy(gameObject, 2f);
        }
        else
        {
            Invoke("ResetMaterial", .1f);
        }    
    }

    void ResetMaterial()
    {
        enemySR.material = matDefault;
    }

}
