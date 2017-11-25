using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnBasedHandler
{

    public string Attacker; // Name of the Attacker
    public string Type; // Type of Attacker
    public GameObject AttackersGameObject; // Who is Attacking
    public GameObject AttackersTarget; // Who is the attackers target

    public BaseAttack choosenAttack;
}
