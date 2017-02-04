using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MyVector2D {

    public int x;
    public int z;
	
    public MyVector2D (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static MyVector2D operator + (MyVector2D a, MyVector2D b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}
