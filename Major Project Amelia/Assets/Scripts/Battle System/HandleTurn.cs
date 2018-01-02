using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    // Variables for Attacking 
    public string AttackersName; //Attackers Name
    public string Type;
    public GameObject AttackersObject; // Attackers Object
    public GameObject AttackersTarget; // Targets Object

    // Attack Damage
    public float attackDamage;

}
