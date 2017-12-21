using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public PerformAction battleState;

    [Header("Lists")]
    public List<HandleTurn> PerformList = new List<HandleTurn>(); // List responsible for attacks and skill usage
    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    void Start()
    {
        // Sets the Battle State to waiting
        battleState = PerformAction.WAIT;
        // Grabs all Heros and Enemies in Battle and adds to list
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        
    }

    void Update()
    {
        switch(battleState)
        {
            case PerformAction.WAIT:
                if (PerformList.Count > 0)
                {
                    battleState = PerformAction.TAKEACTION;
                }

                break;

            case PerformAction.TAKEACTION:
                GameObject performer = GameObject.Find(PerformList[0].AttackersName);
                if(PerformList[0].Type == "Enemy")
                {
                    EnemyState ES = performer.GetComponent<EnemyState>();
                    ES.HeroToAttack = PerformList[0].AttackersTarget;
                    ES.currentState = EnemyState.TurnState.ACTION;
                }
                if (PerformList[0].Type == "Hero")
                {

                }

                battleState = PerformAction.PERFORMACTION;
                break;

            case PerformAction.PERFORMACTION:

                break;
        }

    }

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }
}
