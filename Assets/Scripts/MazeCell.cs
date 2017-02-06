using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeCell : MonoBehaviour {
    public MyVector2D coordinates;
    public int currentEdgeCount;
    public MazeCellEdge[] edges = new MazeCellEdge[MyDirections.Count];
    public int wallCount = 0;
    public Maze myMaze;
    public NavMeshSurface nmsurface;
    public bool isPassable = true;
    public bool isStart = false;
    public bool isThreshold = false;
    public bool isAlcove = false;
    public bool isEnd = false;
    public key keyPrefab;
    public key myKey;
    public MazeCellEdge GetEdge (MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void placeKey()
    {
        myKey = Instantiate(keyPrefab) as key;
        myKey.transform.position = this.transform.position + new Vector3(0f, 0.3f, 0f);
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge mazeCellEdge)
    {
        edges[(int)direction] = mazeCellEdge;
        currentEdgeCount += 1;
    }

    public void DestroyWall(MazeCellEdge edge)
    {
        edge.firstCell.edges[edge.firstCellIndex] = null;
        edge.firstCell.currentEdgeCount--;
        edge.firstCell.wallCount--;
        if (edge.secondCell != null)
        {
            edge.secondCell.wallCount--;
        }
        
        Destroy(edge.gameObject);
    }

    public bool Initialized
    {
        get
        {
            return currentEdgeCount == MyDirections.Count;
        }
    }

    public MazeDirection RandomDir_uninit
    {
        get
        {
            int skips = Random.Range(0, MyDirections.Count - currentEdgeCount);
            for (int i = 0; i < MyDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }
}
