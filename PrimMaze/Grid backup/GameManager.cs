using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Grid mazePrefab;
    private Grid mazeInstance;

	// Use this for initialization
	void Start () {
        BeginGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartGame();
        }
	}

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Grid;
        mazeInstance.Generate();
        // mazeInstance.GenerateMaze();
    }

    private void RestartGame()
    {
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
