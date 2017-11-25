using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass

{
    public string theName;
    // Health
    public float baseHP;
    public float currentHP;

    // Attack
    public float baseAttack;
    public float currentAttack;

    // Defence
    public float baseDefence;
    public float currentDefence;

    // Stamina
    public float baseStamina;
    public float currentStamina;

    // Agility - affects how quickly they can attack
    public float agility;

    public List<BaseAttack> attacks = new List<BaseAttack>();

}

