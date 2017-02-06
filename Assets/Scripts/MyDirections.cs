using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyDirections {

    public const int Count = 4;

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    private static MyVector2D[] myVectors =
    {
        new MyVector2D(0,1),
        new MyVector2D(1,0),
        new MyVector2D(0,-1),
        new MyVector2D(-1,0)
    };

    private static MazeDirection[] myOpposites =
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite (this MazeDirection direction)
    {
        return myOpposites[(int)direction];
    }

    public static MyVector2D dirToVector (this MazeDirection dir)
    {
        return myVectors[(int)dir];
    }

    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }

}
