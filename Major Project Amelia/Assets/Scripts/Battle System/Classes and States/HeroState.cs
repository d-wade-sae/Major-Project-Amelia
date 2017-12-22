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

                break;

            case (TurnState.DEAD):

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
}
