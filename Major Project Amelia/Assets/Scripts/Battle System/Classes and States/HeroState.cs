using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroState : MonoBehaviour {

    public BaseHero hero;

    private BattleManager BM;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState; // Which State the hero is in

    // For the Progress Bar during Prototyping *yawn*
    private float cur_cooldown = 0f;
    private float max_cooldown = 1.5f;

    [Header("Attack Variables")]
    public GameObject EnemyToAttack;
    private Vector3 startPosition; // Start Position for moving
    private bool actionStarted = false;
    public GameObject selector;

    private float animSpeed = 10f;

    void Start ()
    {
        BM = GameObject.Find("_BattleManager").GetComponent<BattleManager>();
        startPosition = transform.position;
        selector.SetActive(false);
        hero.currentHP = hero.baseHP;
        hero.baseStamina = hero.currentStamina;
    }
	
	void Update ()
    {
        print(this.gameObject.name + " is in " + currentState);
        switch (currentState)
        {
            case (TurnState.PROCESSING):

                UpdateProgressBar();

                break;

            case (TurnState.ADDTOLIST):
                BM.HerosToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;
                
            case (TurnState.WAITING):

                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if (!hero.alive)
                {
                    return;
                }
                else
                {
                    // Can't be attacked
                    BM.HerosInBattle.Remove(this.gameObject);
                    // Not able to use hero
                    BM.HerosToManage.Remove(this.gameObject);
                    // De-Activate Selector
                    selector.SetActive(false);
                    // Reset GUI
                    BM.actionPanel.SetActive(false);
                    BM.targetPanel.SetActive(false);

                    // Remove from Perform List
                    if (BM.HerosInBattle.Count > 0)
                    {
                        for (int i = 0; i < BM.PerformList.Count; i++)
                            if (i != 0)
                            {
                                {
                                    if (BM.PerformList[i].AttackersObject == this.gameObject)
                                    {
                                        BM.PerformList.Remove(BM.PerformList[i]);
                                    }

                                    if (BM.PerformList[i].AttackersTarget == this.gameObject)
                                    {
                                        BM.PerformList[i].AttackersTarget = BM.HerosInBattle[Random.Range(0, BM.HerosInBattle.Count)];
                                    }
                                }
                            }
                    }

                    // Change Colour/ Play Animation
                    Destroy(this.gameObject);
                    // Reset Hero Input
                    BM.battleState = BattleManager.PerformAction.CHECKSTATUS;
                    hero.alive = false;
                }
                break;
        }
	}

    void UpdateProgressBar ()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;

        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        // Moving towards Hero to Attack
        actionStarted = true;
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x - 1f, EnemyToAttack.transform.position.y + .6f, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPosition))
        {
            yield return null;
        }

        // Wait for x seconds infront of Hero
        yield return new WaitForSeconds(0.5f);

        // Damage Said Hero
        DoDamage();

        // Move back to start Position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }

        // Remove from the Battle Manager Perform List and EnemiesToManage
        if (BM.battleState == BattleManager.PerformAction.WIN)
        {
            currentState = TurnState.WAITING;
        }

        else
        {
            BM.PerformList.RemoveAt(0);
            // Resetting the Battle Manager State Machine
            BM.battleState = BattleManager.PerformAction.WAIT;
            /*
        // Reset the state machine into wait cycle
        enemyTurn = false;

        if (battleManager.EnemiesToManage.Count == 0)
        {
            battleManager.heroTurns = true;
            battleManager.enemyTurns = false;
            currentState = TurnState.PROCESSING;
        }
        */

            // else
            // {
            currentState = TurnState.PROCESSING;
            cur_cooldown = 0; // this gets removed later when its turn based not ATB
                              // }
        }

        actionStarted = false;
        
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {

        float calculateDamage = hero.baseAttack;
        EnemyToAttack.GetComponent<EnemyState>().TakeDamage(calculateDamage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        hero.currentHP -= getDamageAmount;
        if (hero.currentHP <= 0)
        {
            hero.currentHP = 0;
            currentState = TurnState.DEAD;
        }
    }
}
