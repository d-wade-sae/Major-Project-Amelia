using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("_BattleManager").GetComponent<BattleManager>().Input2(EnemyPrefab); // save input enemy prefab

    }

    public void ShowSelector() 
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(true);

    }

    public void HideSelector()
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false);

    }

}
