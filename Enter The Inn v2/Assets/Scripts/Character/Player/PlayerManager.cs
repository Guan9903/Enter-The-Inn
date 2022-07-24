using System.Collections;
using UnityEngine;

public class PlayerManager : ICharacter
{
    public AudioClip hurtAudio;
    public int playerHealth;
    public float invincibleTime;
    public static bool alive;

    private bool intoInvincible;

    private void Awake()
    {
        ICharacterSet();
        playerHealth = intHealth;
        alive = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetHurt(int intDamage)
    {
        if (intoInvincible)
        {
            playerHealth -= 0;
        }
        else
        {
            
            //source.clip = hurtAudio;
            //source.Play();
            //GetComponent<ScreenFlash>().FlashScreen();
            StartCoroutine(Invincible(intDamage));

            var audios = GetComponents<AudioSource>();
            int t = 0;
            for (int i = 0; i < audios.Length; i++)
            {
                if (audios[i].clip == hurtAudio)
                {
                    audios[i].Play();
                    t++;
                    break;
                }
                
            }

            if (t == 0)
            {
                //当没有多余的音频源空闲时，则创建新的音频源
                AudioSource newAs = gameObject.AddComponent<AudioSource>();
                newAs.loop = false;
                newAs.clip = hurtAudio;
                newAs.Play();
            }
            
        }

        if (playerHealth <= 0)
        {
            alive = false;
            
            Time.timeScale = 0.1f;
            StartCoroutine(GameOver());

            Debug.Log("Player Die!");
        }
        else
            alive = true;

    }

    IEnumerator Invincible(int intDamage)
    {
        playerHealth -= intDamage;
        intoInvincible = true;

        GetComponent<ScreenFlash>().FlashScreen();

        var t = GetComponent<ScreenFlash>().time;

        yield return new WaitForSeconds(t);

        intoInvincible = false;
    }

    IEnumerator GameOver()
    {
        var t = GetComponent<ScreenFlash>().time;
        yield return new WaitForSeconds(t*2);
        GameStatesManager.gameStateController.SetState(new GameOver(GameStatesManager.gameStateController));
    }
}
