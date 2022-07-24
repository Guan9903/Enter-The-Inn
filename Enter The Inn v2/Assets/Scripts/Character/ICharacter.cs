using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacter : MonoBehaviour
{
    public CharacterConfig config;
    public AudioClip dieAudio;

    protected SpriteRenderer enemySR;
    protected AudioSource source;
    protected int id;
    protected string characterName;
    protected int intHealth;
    protected float health;
    protected float speed;

    public void ICharacterSet()
    {
        id = config.id;
        characterName = config.characterName;
        intHealth = config.intHealth;
        health = config.health;
        speed = config.speed;

        enemySR = GetComponentInChildren<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    public virtual void GetHurt(float floatDamage) { }

    public virtual void GetHurt(int intDamage) { }
}
