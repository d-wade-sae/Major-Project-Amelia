using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseClass
{
    public string theName;
    public bool alive = true;
    // Health
    public float baseHP;
    public float currentHP;
    // Stamina
    public float baseStamina;
    public float currentStamina;
    // Attack
    public float baseAttack;
    public float currentAttack;
}
 