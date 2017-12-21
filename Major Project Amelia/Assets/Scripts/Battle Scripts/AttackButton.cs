using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour {

    public BaseAttack skillAttackToPerform;

    public void UseSkill()
    {
        GameObject.Find("_BattleManager").GetComponent<BattleManager>().Input4(skillAttackToPerform);
    }
}
