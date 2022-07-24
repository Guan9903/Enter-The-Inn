using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SPECIES
{
    Player,
    Enemy,
    NPC
}

[CreateAssetMenu]
public class CharacterConfig : ScriptableObject
{
    public SPECIES characterType;
    public int id;
    public string characterName;
    public int intHealth;
    public float health;
    public float speed;
}
