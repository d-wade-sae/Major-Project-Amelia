using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows it to be viewable in the inspector
[System.Serializable]
public class MainHero : BaseClass
{
    public int heroLevel;
    public int heroXP;

    public List<BaseAttack> Skills = new List<BaseAttack>();
}
