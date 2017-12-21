using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    private GameManager GM;

    [Header("Battle Attributes")]
    public static BattleManager instance;
    public GameObject background;

    [Header("Battle Variables")]
    public int escapeChance = 0;
    public bool heroTurns = false;
    public bool enemyTurns = false;
    public bool performTurn = false;
    public bool enableDebugLines = false;
    private TurnBasedHandler HeroChoice;

    // End Battle Variables
    private bool endBattle = false;
    private bool resetBattle = false;
    private bool battleWin = false;
    private bool battleLose = false;
    private bool battleEscape = false;
    private bool escapeBattleFail = false;

    [Header("Player Objects")]
    public GameObject battlePlayer;
    public GameObject battleCompanion;

    [Header("Battle Canvas Master")]
    public GameObject battleCanvas;

    [Header("Prefabs")]
    // Button Prefabs
    public GameObject actionButton;
    public GameObject enemyButton;
    public GameObject skillButton;

    [Header("Panels")]
    public GameObject actionsPanel;
    public GameObject enemySelectPanel;
    public GameObject skillsPanel;
    public GameObject heroPanel;

    [Header("Spacers")]
    public Transform actionSpacer;
    public Transform enemySelectSpacer;
    public Transform skillSpacer;

    [Header("States")]
    public PerformAction battleStates;
    public HeroGUI HeroInput;

    public enum PerformAction // Manages the Battle States and wether anything is performing an action
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE,
        RESET,
        STANDBY
    }

    public enum HeroGUI // Responsible for Hero Turns
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE,
        RESET
    }

    [Header("Lists")]
    public List<TurnBasedHandler> PerformList = new List<TurnBasedHandler>(); // List that handles whos turn it is to perform an action
    public List<GameObject> HerosInGame = new List<GameObject>(); // List that handles which heros are in game
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); // List that handles which enemies are in game
    public List<GameObject> HerosToManage = new List<GameObject>(); //  List that is responsible for which hero turn it is
    public List<GameObject> EnemiesToManage = new List<GameObject>(); // list that is responsible for which enemies need turns
    public List<GameObject> DeadEnemies = new List<GameObject>(); // list of dead enemies (only used for 2 or more enemies)
    
    // Private Lists
    // List for Attack Buttons
    private List<GameObject> attackButtons = new List<GameObject>();
    // List for Enemy Buttons
    private List<GameObject> enemyButtons = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	
	void Update ()
    {
        if (enableDebugLines) print("Battle Manager State: " + battleStates);
        switch (battleStates)
        {
            case (PerformAction.WAIT):

                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }

                if (enemyTurns == true)
                {
                    if (enableDebugLines) print("BATTLE MANAGER: Enemy Turn is True");
                    foreach (GameObject enemy in EnemiesInBattle)
                    {
                        EnemyState ES = enemy.GetComponent<EnemyState>();
                        ES.enemyTurn = true;
                        ES.currentState = EnemyState.TurnState.PROCESSING;
                        if (enableDebugLines) print("BATTLE MANAGER " + enemy.name + " has set enemyTurn to true");
                    }

                    enemyTurns = false;
                }

                if (heroTurns == true)
                {
                    foreach (GameObject hero in HerosInGame)
                    {
                        HeroState MS = hero.GetComponent<HeroState>();
                        MS.playerTurn = true;
                    }
                }

                break;

            case (PerformAction.TAKEACTION):
                if (enableDebugLines) print("Checking the perform list");
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyState enemyState = performer.GetComponent<EnemyState>();
                    for (int i = 0; i < HerosInGame.Count; i++)
                    {
                        if (PerformList[0].AttackersTarget == HerosInGame[i])
                        {
                            enemyState.HeroToAttack = PerformList[0].AttackersTarget;
                            enemyState.currentState = EnemyState.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = HerosInGame[Random.Range(0, HerosInGame.Count)];
                            enemyState.HeroToAttack = PerformList[0].AttackersTarget;
                            enemyState.currentState = EnemyState.TurnState.ACTION;
                        }
                    }

                }

                if (PerformList[0].Type == "Hero")
                {
                    HeroState heroState = performer.GetComponent<HeroState>();
                    heroState.enemyToAttack = PerformList[0].AttackersTarget;
                    heroState.currentState = HeroState.TurnState.ACTION;
                }

                if (enableDebugLines) print(performer + " has been collected from the perform action");

                battleStates = PerformAction.PERFORMACTION;

                break;

            case (PerformAction.PERFORMACTION):

                break;

            case (PerformAction.CHECKALIVE): // Checks to see whos alive in battle and determine if won/lost/next turn
                if (HerosInGame.Count == 0 || battlePlayer.GetComponent<HeroState>().alive == false)
                {
                    battleStates = PerformAction.LOSE;
                }

                else if (EnemiesInBattle.Count == 0)
                {
                    battleStates = PerformAction.WIN;
                }

                else
                {
                    ClearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                }

                break;

            case (PerformAction.WIN):
                {
                    battleWin = true;
                    if (resetBattle == false)
                    {
                        // StartCoroutine(EndBattleSequence());
                    }
                }
                break;

            case (PerformAction.LOSE):
                {
                    ClearAttackPanel();
                    battleLose = false;
                    
                    // Load end Scene
                }
                break;

            case (PerformAction.RESET):

                break;

            case (PerformAction.STANDBY):

                break;
        }

        switch (HeroInput)
        {

            case (HeroGUI.ACTIVATE):
                if (HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new TurnBasedHandler();

                    actionsPanel.SetActive(true);
                    CreateActionButtons();
                    HeroInput = HeroGUI.WAITING;
                }

                break;

            case (HeroGUI.WAITING):

                break;

            case (HeroGUI.DONE):

                HeroInputDone();
                break;

            case (HeroGUI.RESET):

                print("Hero Input in battlemanager in reset state");

                break;
        }
    }

    public void StartBattle() // Everything for Starting the Battle
    {
        // Enables player and Companion
        battlePlayer.SetActive(true);
        battleCompanion.SetActive(true);
        // Enables Battle Canvas
        battleCanvas.SetActive(true);
        // Enables all panels for spawning of buttons
        actionsPanel.SetActive(true);
        enemySelectPanel.SetActive(true);
        skillsPanel.SetActive(true);

        battleStates = PerformAction.WAIT;
        HeroInput = HeroGUI.ACTIVATE;

        HerosInGame.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        // Spawns in All Buttons 
        EnemyButtons();
        
    }

    // Not enabled now
    void SpawnEnemies()
    {
        
    }

    public void CollectActions(TurnBasedHandler input)
    {
        PerformList.Add(input);
    }

    public void EnemyButtons()
    {
        // Cleaning Up Buttons
        foreach (GameObject enemyBtn in enemyButtons)
        {
            Destroy(enemyBtn);
        }

        enemyButtons.Clear();

        // Creating Buttons
        foreach (GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            newButton.transform.SetParent(enemySelectSpacer);
            newButton.transform.localScale = new Vector3(1f, 1f, 1f);
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyState currentEnemy = enemy.GetComponent<EnemyState>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = currentEnemy.enemy.theName;

            button.EnemyPrefab = enemy;
            enemyButtons.Add(newButton);

            if (enableDebugLines) print("Spawning Buttons");
        }

    }

    public void Input1() // Attack Button 
    {
        // Begins collecting data for hero attack 
        if (EnemiesInBattle.Count == 0)
        {
            return;
        }

        else
        {
            HeroChoice.Attacker = HerosToManage[0].name;
            HeroChoice.AttackersGameObject = HerosToManage[0];
            HeroChoice.Type = "Hero";
            HeroChoice.choosenAttack = HerosInGame[0].GetComponent<HeroState>().hero.attacks[0];

            // Disables Action Panel and Enables Enemy Target Panel
            actionsPanel.SetActive(false);
            enemySelectPanel.SetActive(true);
        }

    }

    public void Input2(GameObject targetEnemy) // Enemy Target Selection
    {
        HeroChoice.AttackersTarget = targetEnemy;
        HeroInput = HeroGUI.DONE;
    }

    public void Input4(BaseAttack choosenSkill) // Choosen Skill to Use
    {
        // Begins collecting data for hero attack
        HeroChoice.Attacker = HerosToManage[0].name;
        HeroChoice.AttackersGameObject = HerosToManage[0];
        HeroChoice.Type = "Hero";

        HeroChoice.choosenAttack = choosenSkill;
        HeroChoice.AttackersGameObject.GetComponent<HeroState>().hero.currentStamina = HeroChoice.AttackersGameObject.GetComponent<HeroState>().hero.currentStamina - HeroChoice.choosenAttack.attackCost;
        skillsPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void Input3() // Switching to Skills Attack
    {
        actionsPanel.SetActive(false);
        skillsPanel.SetActive(true);
    }

    public void BackToActionScreen() // Back to selecting what action to take
    {
        enemySelectPanel.SetActive(false);
        actionsPanel.SetActive(true);
    }

    public void HeroInputDone() // Completed Hero Input and sending info to the perform list plus removing hero from HerosToManage
    {
        PerformList.Add(HeroChoice);
        ClearAttackPanel();

        HerosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HerosToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    /*
    public void Escape() // Escaping Battle Function
    {
        int randomNumber = Random.Range(0, 100);

        if (escapeChance > randomNumber) // if escape chance is higher than random number, then the player escapes, if not he is stuck in battle
        {
            battleEscape = true;
            StartCoroutine(EndBattleSequence());
        }

        else
        {
            endBattleText.gameObject.SetActive(true);
            endBattleText.text = "You have failed to escape";

            ClearAttackPanel();

            StartCoroutine(EscapeFail());

        }
    }
    */

    void ClearAttackPanel()
    {
        enemySelectPanel.SetActive(false);
        actionsPanel.SetActive(false);
        skillsPanel.SetActive(false);
        foreach (GameObject AttackButton in attackButtons)
        {
            Destroy(AttackButton);
        }
        attackButtons.Clear();
    }

    /*
    public IEnumerator EndBattleSequence() // ENDING AND RESETING BATTLE STEP 1
    {
        if (endBattle)
        {
            yield break;
        }

        resetBattle = true;

        endBattle = true;
        ClearAttackPanel();
        battleStates = PerformAction.RESET;
        HeroInput = HeroGUI.RESET;
        if (enableDebugLines) print("Hero Input is now Waiting");


        for (int i = 0; i < HerosInGame.Count; i++)
        {
            HeroState MHS = HerosInGame[i].GetComponent<HeroState>();
            MHS.UpdateHeroPanel();
            MHS.UpdatePlayerLevel();
            MHS.enemyToAttack = null;

        }
        print("Has completed the for loop");

        // Clearing All of the Lists
        PerformList.Clear();
        HerosInGame.Clear();
        HerosToManage.Clear();
        EnemiesToManage.Clear();

        foreach (GameObject enemy in EnemiesInBattle) // Destroys All Enemies in Battle
        {
            Destroy(enemy);
        }

        foreach (GameObject enemy in DeadEnemies) // Destroys All Dead Enemies in Battle
        {
            Destroy(enemy);
        }
        EnemiesInBattle.Clear(); // Clears the Enemies in Battle List

        if (enableDebugLines) print("has cleared the perform list, heros in game and heros to manage");

        if (battleWin == true)
        {
            endBattleText.gameObject.SetActive(true);
            endBattleText.text = "Enemies have been defeated!";
        }

        else if (battleLose == true)
        {
            endBattleText.gameObject.SetActive(true);
            endBattleText.text = "You have been defeated!";
        }

        else if (battleEscape == true)
        {
            endBattleText.gameObject.SetActive(true);
            endBattleText.text = "You have escaped Battle!";
        }

        if (enableDebugLines) print("displayed the battle result text");

        // yield return new WaitForSeconds(3); // THIS LINE BREAKS EVERYTHING
        // print("has waited 3 seconds");

        
        playerWorld.SetActive(true);
        worldCamera.SetActive(true);
        print("EndBattleSequence End");
        
        if (enableDebugLines) print("Calling Battle End");
        // BattleEnd();
        endBattle = false;
        resetBattle = false;

    }
    */

    /*
    public IEnumerator EscapeFail()
    {
        if (escapeBattleFail)
        {
            yield break;
        }

        escapeBattleFail = true;

        yield return new WaitForSeconds(3f);

        endBattleText.text = null;
        endBattleText.gameObject.SetActive(false);

        heroTurns = false;
        HerosToManage[0].GetComponent<HeroState>().playerTurn = false;
        enemyTurns = true;
        HerosToManage[0].GetComponent<HeroState>().currentState = HeroState.TurnState.PROCESSING;
        HerosToManage.Clear();
        HeroInput = HeroGUI.ACTIVATE;

        escapeBattleFail = false;
    }
    */



    /*
    public void BattleEnd()
    {
        if (enableDebugLines) print("Battle End Starting");
        HerosToManage.Clear();

        gameManager.GetComponent<GameManager>().BattleReset();

        if (enableDebugLines) print("Battle End finished");
        

        // everything below this as well needs to be commented
        battleCamera.SetActive(false);
        actionsPanel.SetActive(false);
        enemySelectPanel.SetActive(false);
        endBattleText.gameObject.SetActive(false);
        battleArea.SetActive(false);
        worldParent.SetActive(true);
        worldCamera.SetActive(true);
        
        print("Battle End");
        
    }
    */



    void CreateActionButtons()
    {
        // Attack Button
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("ActionText").GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(AttackButton);


        // Skills Button
        GameObject SkillButton = Instantiate(actionButton) as GameObject;
        Text SkillButtonText = SkillButton.transform.Find("ActionText").GetComponent<Text>();
        SkillButtonText.text = "Skills";
        SkillButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        SkillButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(SkillButton);

        if (HerosInGame[0].GetComponent<HeroState>().hero.Skills.Count > 0)
        {
            foreach (BaseAttack skillAttack in HerosInGame[0].GetComponent<HeroState>().hero.Skills)
            {
                GameObject selectSkillButton = Instantiate(skillButton) as GameObject;
                Text selectSkillButtonText = selectSkillButton.transform.Find("SkillText").GetComponent<Text>();
                selectSkillButtonText.text = skillAttack.attackName;
                AttackButton ATB = selectSkillButton.GetComponent<AttackButton>();
                ATB.skillAttackToPerform = skillAttack;
                selectSkillButton.transform.SetParent(skillSpacer, false);
                if (skillAttack.attackCost < HerosInGame[0].GetComponent<HeroState>().hero.currentStamina)
                {
                    attackButtons.Add(selectSkillButton);
                }
            }
        }
        else
        {
            SkillButton.GetComponent<Button>().interactable = false;
        }

        /*
        // Inventory Button
        GameObject InventoryButton = Instantiate(actionButton) as GameObject;
        Text InventoryButtonText = InventoryButton.transform.Find("ActionText").GetComponent<Text>();
        InventoryButtonText.text = "Inventory";

        InventoryButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(InventoryButton);
        */

        /*
        // Escape Button
        GameObject EscapeButton = Instantiate(actionButton) as GameObject;
        Text EscapeButtonText = EscapeButton.transform.Find("ActionText").GetComponent<Text>();
        EscapeButtonText.text = "Escape!";
        EscapeButton.GetComponent<Button>().onClick.AddListener(() => Escape());

        EscapeButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(EscapeButton);
        */
    }
}
