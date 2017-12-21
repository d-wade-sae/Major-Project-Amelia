using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroState : MonoBehaviour {

    public BaseHero hero;
    
    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState; // Which State the hero is in

    // For the Progress Bar during Prototyping *yawn*
    private float cur_cooldown = 0f;
    private float max_cooldown = 1.5f;
    
	void Start ()
    {
		
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

                break;
                
            case (TurnState.WAITING):

                break;

            case (TurnState.SELECTING):

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
