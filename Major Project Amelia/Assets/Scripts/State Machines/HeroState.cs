using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour {

    private BattleManager BM;
    public MainHero hero;
    public bool battlePlayer;
    private bool inBattle;
    public GameObject characterWorld;

    public bool playerTurn;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    public GameObject selector;

    // Attack Movements
    public GameObject enemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 10f;
    public bool alive = true;

    void Start ()
    {
        BM = GameObject.Find("BattleMaster").GetComponent<BattleManager>();

        currentState = TurnState.PROCESSING;
        selector.SetActive(false);
        startPosition = transform.position;
	}
	
	void Update ()
    {
        if (BM.battleStates == BattleManager.PerformAction.WAIT && BM.HeroInput == BattleManager.HeroGUI.ACTIVATE)
        {
            //currentState = TurnState.PROCESSING;
            //enemyToAttack = null;
        }
        // this is checking that the battle is over with and then resetting the player and killing the TimeForAction coroutine
        if (BM.battleStates == BattleManager.PerformAction.WIN && BM.HeroInput == BattleManager.HeroGUI.WAITING)
        {
            currentState = TurnState.PROCESSING;
            enemyToAttack = null;
            StopCoroutine(TimeForAction());
            if (actionStarted)
            {
                if (enableDebug) Debug.Log("TimeForAction: KILLED!");
            }
            actionStarted = false;  // make sure to set to false otherwise coroutine will never start properly          
        }

        //if (enableDebug) Debug.Log("Player State: " + currentState);
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                if (playerTurn)
                {
                    UpdateHeroPanel();
                    currentState = TurnState.ADDTOLIST;
                }

                break;

            case (TurnState.ADDTOLIST):
                BM.HerosToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;

                break;

            case (TurnState.WAITING): // Idle State

                if (BM.battleStates == BattleManager.PerformAction.PERFORMACTION && BM.HeroInput == BattleManager.HeroGUI.WAITING)
                {
                    //currentState = TurnState.ACTION;
                }

                break;

            case (TurnState.ACTION): // Action State
                if (BM.HeroInput == BattleManager.HeroGUI.WAITING && BM.battleStates == BattleManager.PerformAction.WIN)
                {
                    //enemyToAttack = null;
                    //currentState = TurnState.WAITING;
                }
                if (enemyToAttack != null) StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if (!alive)
                {
                    return;
                }
                else
                {
                    // Change Tag
                    this.gameObject.tag = "DeadHero";
                    // Can't be attacked
                    BM.HerosInGame.Remove(this.gameObject);
                    // Not able to use hero
                    BM.HerosToManage.Remove(this.gameObject);
                    // De-Activate Selector
                    selector.SetActive(false);
                    // Reset GUI
                    BM.actionsPanel.SetActive(false);
                    BM.enemySelectPanel.SetActive(false);

                    // Remove from Perform List
                    if (BM.HerosInGame.Count > 0)
                    {
                        for (int i = 0; i < BM.PerformList.Count; i++)
                            if (i != 0)
                            {
                                {
                                    if (BM.PerformList[i].AttackersGameObject == this.gameObject)
                                    {
                                        BM.PerformList.Remove(BM.PerformList[i]);
                                    }

                                    if (BM.PerformList[i].AttackersTarget == this.gameObject)
                                    {
                                        BM.PerformList[i].AttackersTarget = BM.HerosInGame[Random.Range(0, BM.HerosInGame.Count)];
                                    }
                                }
                            }
                    }

                    // Change Colour/ Play Animation
                    this.gameObject.SetActive(false);
                    // Reset Hero Input
                    BM.battleStates = BattleManager.PerformAction.CHECKALIVE;


                    alive = false;
                }

                break;
        }
    }

    private IEnumerator TimeForAction()
    {

        if (actionStarted)
        {
            yield break;
        }
        if (enableDebug) Debug.Log("TimeForAction: STARTED!");

        // Moving towards Hero to Attack
        actionStarted = true;
        // this makes sure that the enemy object is not missing or null reference before doing this section to avoid errors
        if (enemyToAttack != null)
        {
            Vector3 enemyPosition = new Vector3(enemyToAttack.transform.position.x - 1.5f, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z);
            while (MoveTowardsEnemy(enemyPosition))
            {
                yield return null;
            }

            // Wait for x seconds infront of Hero
            yield return new WaitForSeconds(0.5f);

            // Damage Target
            DoDomage();

            // Move back to start Position
            Vector3 firstPosition = startPosition;
            while (MoveTowardsStart(firstPosition))
            {
                yield return null;
            }

            // Remove from the Battle Manager Perform List
            for (int i = 0; i < BM.PerformList.Count; i++)
            {
                BM.PerformList.RemoveAt(i);
            }

            if (BM.battleStates != BattleManager.PerformAction.WIN && BM.battleStates != BattleManager.PerformAction.LOSE)
            {
                // Resetting the Battle Manager State Machine
                BM.battleStates = BattleManager.PerformAction.WAIT;

                // Reset the state machine into wait cycle
                playerTurn = false;
                if (BM.HerosToManage.Count == 0)
                {
                    BM.heroTurns = false;
                    BM.enemyTurns = true;
                }

                currentState = TurnState.PROCESSING;
            }

            else
            {
                currentState = TurnState.WAITING;
            }

            if (enableDebug) Debug.Log("Player State: " + currentState);

            UpdatePlayerStats();
        }

        actionStarted = false;

        if (enableDebug) Debug.Log("TimeForAction: ENDED!");
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDomage()
    {
        float calculateDamage = hero.currentAttack + BM.PerformList[0].choosenAttack.attackDamage;
        enemyToAttack.GetComponent<EnemyState>().TakeDamage(calculateDamage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        hero.currentHP -= getDamageAmount;
        if (hero.currentHP <= 0)
        {
            hero.currentHP = 0;
            currentState = TurnState.DEAD;
        }

        UpdateHeroPanel();
        UpdatePlayerStats();
    }

    void CreateHeroPanel()
    {
        heroPanel = Instantiate(heroPanel) as GameObject;
        heroPanelStats = heroPanel.GetComponent<HeroPanelStats>();
        heroPanelStats.heroName.text = hero.theName;
        heroPanelStats.heroHP.text = "HP: " + hero.currentHP + "/" + hero.baseHP;
        heroPanelStats.heroSTA.text = "STA: " + hero.currentStamina + "/" + hero.baseStamina;
        heroPanelStats.heroLVL.text = "LVL: " + hero.heroLevel;
        heroPanelStats.heroXP.text = "XP: " + hero.heroXP;

        heroPanel.transform.SetParent(heroPanelSpacer, false);

    }

    /*
    public void UpdateHeroPanel()
    {
        heroPanelStats = heroPanel.GetComponent<HeroPanelStats>();
        heroPanelStats.heroHP.text = "HP: " + hero.currentHP + "/" + hero.baseHP;
        heroPanelStats.heroSTA.text = "STA: " + hero.currentStamina + "/" + hero.baseStamina;
    }
    */

    /*
    public void UpdatePlayerStats()
    {
        if (isBattlePlayer)
        {
            playerWorld.GetComponent<PlayerStats>().playerHealth = hero.currentHP;
            playerWorld.GetComponent<PlayerStats>().stamina = hero.currentStamina;
            playerWorld.GetComponent<PlayerStats>().playerLevel = hero.heroLevel;
            playerWorld.GetComponent<PlayerStats>().currentXP = hero.heroXP;

            if (enableDebug) Debug.Log("Updated Player Stats");
        }

        else
        {
            if (enableDebug) Debug.Log("Not updating Player Stats");
        }
    }

    public void UpdatePlayerLevel()
    {
        if (isBattlePlayer)
        {
            hero.heroLevel = playerWorld.GetComponent<PlayerStats>().playerLevel;
            hero.heroXP = playerWorld.GetComponent<PlayerStats>().currentXP;
        }
    }
    */
}
}
