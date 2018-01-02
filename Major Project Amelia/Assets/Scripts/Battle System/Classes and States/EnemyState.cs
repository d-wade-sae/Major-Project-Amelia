using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour {

    public BaseEnemy enemy; // Links it to its stats

    private BattleManager BM;

    // Attack Variables
    private Vector3 startPosition; // Start Position for moving
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 10f;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState; // Which State the hero is in

    // For the Progress Bar during Prototyping *yawn*
    private float cur_cooldown = 0f;
    public float max_cooldown = 2.5f;

    void Start()
    {
        BM = GameObject.Find("_BattleManager").GetComponent<BattleManager>();
        startPosition = transform.position;
        enemy.currentHP = enemy.baseHP;
        enemy.baseStamina = enemy.currentStamina;
    }

    void Update()
    {
        print(this.gameObject.name + " is in " + currentState);
        switch (currentState)
        {
            case (TurnState.PROCESSING):

                UpdateProgressBar();

                break;

            case (TurnState.CHOOSEACTION):

                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):

                break;

            case (TurnState.SELECTING):

                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if (!enemy.alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = ("DeadEnemy");
                    BM.EnemiesInBattle.Remove(this.gameObject);

                    if (BM.EnemiesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BM.PerformList.Count; i++)
                        {
                            if (i != 0)
                            {
                                if (BM.PerformList[i].AttackersObject == this.gameObject)
                                {
                                    BM.PerformList.Remove(BM.PerformList[i]);
                                }
                                if (BM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    if (BM.EnemiesInBattle.Count == 0)
                                    {
                                        BM.PerformList[i].AttackersTarget = null;
                                    }

                                    else
                                    {
                                        BM.PerformList[i].AttackersTarget = BM.EnemiesInBattle[Random.Range(0, BM.EnemiesInBattle.Count)];
                                    }
                                }


                            }
                        }
                    }
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    enemy.alive = false;
                    BM.EnemyButtons();

                    BM.battleState = BattleManager.PerformAction.CHECKSTATUS;
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    void UpdateProgressBar() // Used for prototype purposes only (ATB System)
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;

        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.AttackersName = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersObject = this.gameObject;
        myAttack.AttackersTarget = BM.HerosInBattle[Random.Range(0, BM.HerosInBattle.Count)];
        BM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        // Moving towards Hero to Attack
        actionStarted = true;
        Vector3 heroPosition = new Vector3(HeroToAttack.transform.position.x + 1f, HeroToAttack.transform.position.y - .6f, HeroToAttack.transform.position.z);
        while (MoveTowardsEnemy(heroPosition))
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
        BM.PerformList.RemoveAt(0);

        // Resetting the Battle Manager State Machine
        BM.battleState = BattleManager.PerformAction.WAIT;
        
        actionStarted = false;
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

        float calculateDamage = enemy.baseAttack;
        HeroToAttack.GetComponent<HeroState>().TakeDamage(calculateDamage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        enemy.currentHP -= getDamageAmount;
        if (enemy.currentHP <= 0)
        {
            enemy.currentHP = 0;
            currentState = TurnState.DEAD;
        }
    }

}