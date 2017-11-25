using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Allows it to be viewable in the inspector
[System.Serializable]
public class BaseEnemy : BaseClass
{
    public enum Type
    {
        PIG,
        WOLF,
        EAGLE,
        CENTIPEDE,
        BAT
    }

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE
    }

    public Type enemyType;
    public Rarity rarity;

    public int enemyExperience;
}
