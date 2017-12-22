using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	
    [Header("State Machines")]
    public PerformAction battleState;
    // Enum for Battle State
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }
    public HeroGUI HeroInput;
    // Enum for Hero Input;
    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    [Header("Lists")]
    public List<HandleTurn> PerformList = new List<HandleTurn>(); // List responsible for attacks and skill usage
    public List<GameObject> HerosInBattle = new List<GameObject>(); // List contains all heros in battle
    public List<GameObject> EnemiesInBattle = new List<GameObject>(); // List contains all enemies in battle
    public List<GameObject> HerosToManage = new List<GameObject>(); // List controlling which heros need input by player

    [Header("Battle Canvas and Panels")]
    public GameObject battleCanvas;
    public GameObject actionPanel;
    public GameObject targetPanel;

    [Header("Prefabs")]
    public GameObject enemyButton;

    [Header("Spacers")]
    public Transform targetSpacer;

    // Private Variables
    private HandleTurn HeroChoice;

    void Start()
    {
        // Sets the Battle State to waiting
        battleState = PerformAction.WAIT;
        // Grabs all Heros and Enemies in Battle and adds to list
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));

        actionPanel.SetActive(false);
        targetPanel.SetActive(false);

        // Spawns Buttons
        EnemyButtons();
        
    }

    void Update()
    {
        switch(battleState) // SWITCHING BATTLE STATE
        {
            case PerformAction.WAIT:
                if (PerformList.Count > 0)
                {
                    battleState = PerformAction.TAKEACTION;
                }

                break;

            case PerformAction.TAKEACTION:
                GameObject performer = GameObject.Find(PerformList[0].AttackersName); // grabs the performer at top of performlist 
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

        switch (HeroInput)
        {
            case HeroGUI.ACTIVATE:
                if(HerosToManage.Count > 0)
                {
                    HerosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();

                    actionPanel.SetActive(true);
                    HeroInput = HeroGUI.WAITING;
                }
                break;

            case HeroGUI.DONE:

                break;

            case HeroGUI.INPUT1:

                break;

            case HeroGUI.INPUT2:

                break;

            case HeroGUI.WAITING:

                break;
        }

    }

    public void CollectActions(HandleTurn input) // Passes Input from Enemy and Hero into the Perform List
    {
        PerformList.Add(input);
    }

    void EnemyButtons() // Spawns all Enemy Buttons for player to target enemys with
    {
        foreach (GameObject enemy in EnemiesInBattle) 
        {
            GameObject newButton = Instantiate(enemyButton);
            newButton.transform.SetParent(targetSpacer);
            newButton.transform.localScale = new Vector3(1f, 1f, 1f);
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyState currentEnemy = enemy.GetComponent<EnemyState>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = currentEnemy.enemy.name;

            button.EnemyPrefab = enemy;
        }
    }
}
