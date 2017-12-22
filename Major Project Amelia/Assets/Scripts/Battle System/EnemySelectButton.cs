using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("_BattleManager").GetComponent<BattleManager>().Input2(EnemyPrefab);
        
    }
}
