using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;

    private Maze mazeInstance;

    public Canvas myCanvas;

    public Text waitMessage;

	// Use this for initialization
	void Start () {
        BeginGame();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void BeginGame()
    {
        waitMessage.text = "";
        mazeInstance = Instantiate(mazePrefab) as Maze;
        StartCoroutine(mazeInstance.GenerateCells());
    }
}
