using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour {

    public Transform cellPrefab;
    public Vector3 gridSize;
    public Transform[,] grid;
    public List<Transform> maze;
    public List<List<Transform>> frontiers;
	// Use this for initialization
	void Start () {
        CreateGrid();
        createWeights();
        setNeighbors();
        CreateStart();
        nextChoice();
        
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
    }

    void CreateGrid()
    {
        grid = new Transform[(int)gridSize.x, (int)gridSize.z];
        // Basic grid first created
        for (int z = 0; z < gridSize.z; z++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Transform cell;
                cell = (Transform) Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                cell.parent = gameObject.transform;
                cell.name = "Cell " + x + ", " + z;
                cell.GetComponent<Cell>().gridIndex = new Vector3(x, 0, z);
                grid[x, z] = cell;
            }
        }
        Camera.main.transform.position = grid[(int)(gridSize.x / 2), (int) (gridSize.z / 2)].position + Vector3.up*30f;
    }

    void createWeights()
    {
        foreach(Transform child in gameObject.transform)
        {
            int weight = Random.Range(0, 10);
            child.GetComponentInChildren<TextMesh>().text = weight.ToString() ;
            child.GetComponent<Cell>().weight = weight;
        }
    }

    void setNeighbors()
    {
        for (int z = 0; z < gridSize.z; z++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Transform cell;
                cell = grid[x, z];
                Cell cellComponent = cell.GetComponent<Cell>();
                if (x - 1 >= 0)
                {
                    cellComponent.neighbors.Add(grid[x - 1, z]);
                }
                if (x + 1 < gridSize.x)
                {
                    cellComponent.neighbors.Add(grid[x + 1, z]);
                }
                if (z - 1 >= 0)
                {
                    cellComponent.neighbors.Add(grid[x, z - 1]);
                }
                if (z + 1 < gridSize.z)
                {
                    cellComponent.neighbors.Add(grid[x, z + 1]);
                }
                cellComponent.neighbors.Sort(sortWeights);
            }
        }
    }
    
    int sortWeights(Transform cellA, Transform cellB)
    {
        int weightA = cellA.GetComponent<Cell>().weight;
        int weightB = cellB.GetComponent<Cell>().weight;
        return weightA.CompareTo(weightB);
    }

    void CreateStart()
    {
        maze = new List<Transform>();
        frontiers = new List<List<Transform>>();
        for (int i = 0; i < 10; i++)
        {
            frontiers.Add(new List<Transform>());
        }
        int startZIndex = Random.Range(1, (int) gridSize.z - 2);
        Transform start = grid[0, startZIndex];
        start.GetComponent<Renderer>().material.color = Color.green;
        start.GetComponent<Cell>().isStart = true;
        updateMaze(grid[0, startZIndex]);
    }

    void CreateEnd()
    {
        int endZIndex = Random.Range(1, (int)gridSize.z - 2);
        Transform end = grid[(int) gridSize.x - 1, endZIndex];
        end.GetComponent<Cell>().isEnd = true;
    }

    void updateMaze(Transform cell)
    {
        maze.Add(cell);
        foreach (Transform neighbor in cell.GetComponent<Cell>().neighbors)
        {
            neighbor.GetComponent<Cell>().neighborsVisited++;
            if (!maze.Contains(neighbor) && !(frontiers[neighbor.GetComponent<Cell>().weight].Contains(neighbor)))
            {
                frontiers[neighbor.GetComponent<Cell>().weight].Add(neighbor);
            }
        }
    }

    void nextChoice()
    {
        Transform nextCell;
        do
        {
            bool empty = true;
            int leastWeight = 0;
            for (int i = 0; i < 10; i++)
            {
                leastWeight = i;
                if (frontiers[i].Count > 0)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                CancelInvoke("nextChoice");
                CreateEnd();
                clearPath();
                return;
            }
            nextCell = frontiers[leastWeight][0];
            frontiers[leastWeight].Remove(nextCell);
        } while (nextCell.GetComponent<Cell>().neighborsVisited >= 2);
        updateMaze(nextCell);
        Invoke("nextChoice", 0);
    }
    void clearPath()
    {
        
        for (int i = 0; i < maze.Count; i++)
        {
            if ((maze[i].GetComponent<Cell>().gridIndex.x == 0f || maze[i].GetComponent<Cell>().gridIndex.x == gridSize.x - 1 ||
                maze[i].GetComponent<Cell>().gridIndex.z == 0f || maze[i].GetComponent<Cell>().gridIndex.z == gridSize.z - 1)
                && (!maze[i].GetComponent<Cell>().isStart && !maze[i].GetComponent<Cell>().isEnd))
            {

            }
            else
            {
                Destroy(maze[i].gameObject);
            }
        }
        colorMaze();
    }

    void colorMaze()
    {
        foreach (Transform wall in grid)
        {
            if (!maze.Contains(wall))
            {
                wall.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
}
