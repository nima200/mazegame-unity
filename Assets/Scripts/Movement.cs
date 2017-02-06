using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {

    Vector3 goal;
    NavMeshAgent player;

    // Use this for initialization
    void Start () {
        goal = new Vector3(0f, 0f, 0f);
        player = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            goal += transform.forward * player.speed * 0.5f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            goal += transform.right * player.speed * 0.5f;
        }
        player.Move(goal);
	}
}
