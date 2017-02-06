using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public List<Transform> neighbors;
    public Vector3 gridIndex;
    public int weight;
    public int neighborsVisited = 0;
    public bool isStart = false;
    public bool isEnd = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
