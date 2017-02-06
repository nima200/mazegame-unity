using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallMetrics {
    public static Vector3[] positions =
    {
        new Vector3(0f, 0.5f, 0.475f),
        new Vector3(0.475f, 0.5f, 0f),
        new Vector3(0f, 0.5f, -0.475f),
        new Vector3(-0.475f, 0.5f, 0f)
    };

    public static Quaternion[] rotations =
    {
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 0f, 0f),
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 0f, 0f)
    };
}
