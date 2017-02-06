using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour {

    public List<key> myKeys = new List<key>();
	
    public void takeKey(key key)
    {
        myKeys.Add(key);
    }

    public bool canLeave()
    {
        return (myKeys.Count == 3);
    }
}
