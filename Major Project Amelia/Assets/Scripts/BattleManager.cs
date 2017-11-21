using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    private GameManager GM;

    [Header("Battle Canvas")]
    public GameObject battleCanvas;

	void Start ()
    {
        GM = GameObject.Find("_GameManager").GetComponent<GameManager>();
	}
	
	
	void Update ()
    {
		
	}
}
