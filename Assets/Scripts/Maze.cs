using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class Maze : MonoBehaviour {
    public MyVector2D size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public List<MazeWall> allWalls = new List<MazeWall>();
    public float generationStopDelay;
    public int alcoveCount = 0;

    void setStart()
    {
        int randomZIndex = Random.Range(0, size.x - 1);
        if (!cells[0, randomZIndex].isAlcove)
        {
            cells[0, randomZIndex].isStart = true;
            cells[0, randomZIndex].DestroyWall(cells[0, randomZIndex].edges[3]);
        } else
        {
            setStart();
        }
        
    }

    void setEnd()
    {
        int randomZIndex = Random.Range(0, size.x - 1);
        if (!cells[size.x - 1, randomZIndex].isAlcove)
        {
            cells[size.x - 1, randomZIndex].isEnd = true;
            cells[size.x - 1, randomZIndex].DestroyWall(cells[size.x - 1, randomZIndex].edges[1]);
        }
        else
        {
            setEnd();
        }
    }

    public MyVector2D RandomCoordinates {
        get
        {
            return new MyVector2D(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates (MyVector2D coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.z;
    }

    public MazeCell GetCell (MyVector2D coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public IEnumerator GenerateCells()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStopDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
        // DELETE ALL DEAD-ENDS
        
        countAlcoves();
        DestroyDeadEnds();
        colorAlcoves();
        setStart();
        setEnd();
        bakeNavMesh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bakeNavMesh();
        }
    }

    void bakeNavMesh()
    {
        foreach (MazeCell cell in cells)
        {
            cell.nmsurface.Bake();
        }
    }

    void colorAlcoves()
    {

        foreach(MazeCell cell in cells)
        {
            if (cell.wallCount == 3)
            {
                cell.GetComponentInChildren<Renderer>().material.color = Color.black;
                cell.isAlcove = true;
                cell.placeKey();
                //Destroy(cell.GetComponentInChildren<NavMeshSurface>());
                //cell.gameObject.AddComponent<NavMeshObstacle>();
                //cell.gameObject.GetComponent<NavMeshObstacle>().carving = true;
                //cell.gameObject.GetComponent<NavMeshObstacle>().shape = NavMeshObstacleShape.Box;
                //cell.gameObject.GetComponent<NavMeshObstacle>().center = new Vector3(0f,0f,0f);
                //cell.gameObject.GetComponent<NavMeshObstacle>().size = new Vector3(1f, 1f, 1f);
                //cell.gameObject.GetComponent<NavMeshObstacle>().carvingMoveThreshold = 0.1f;
                //cell.gameObject.GetComponent<NavMeshObstacle>().carvingTimeToStationary = 0f;
                //cell.gameObject.GetComponent<NavMeshObstacle>().carveOnlyStationary = false;

            }
        }
    }

    void countAlcoves()
    {
        alcoveCount = 0;
        foreach (MazeCell cell in cells)
        {
            if (cell.wallCount == 3) {
                alcoveCount++;
            }
        }
    }

    void DestroyDeadEnds()
    {
        while(allWalls.Count > 0)
        {
            int randomIndex = Random.Range(0, allWalls.Count - 1);
            MazeWall wall = allWalls[randomIndex];
            if (wall.firstCell == null || wall.secondCell == null)
            {
                allWalls.Remove(wall);
                continue;
            } else
            {
                if (wall.firstCell.wallCount == 3 && alcoveCount > 3)
                {
                    wall.firstCell.DestroyWall(wall);
                }
                if (wall.secondCell.wallCount == 3 && alcoveCount > 3)
                {
                    wall.secondCell.DestroyWall(wall);
                }
                allWalls.Remove(wall);
                countAlcoves();
            }
        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int index = Random.Range(0, activeCells.Count - 1);
        MazeCell cell = activeCells[index];
        if (cell.Initialized)
        {
            activeCells.RemoveAt(index);
            return;
        }
        MazeDirection direction = cell.RandomDir_uninit;
        MyVector2D coordinates = cell.coordinates + direction.dirToVector();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(cell, neighbor, direction);
                activeCells.Add(neighbor);
            } else
            {
                CreateWall(cell, neighbor, direction);
            }
        } else
        {
            CreateWall(cell, null, direction);
        }
    }

    public void CreatePassage(MazeCell firstCell, MazeCell secondCell, MazeDirection dir)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(firstCell, secondCell, dir);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(secondCell, firstCell, dir.GetOpposite());
    }
    
    public void CreateWall(MazeCell firstCell, MazeCell secondCell, MazeDirection dir)
    {
        firstCell.wallCount++;
        
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(firstCell, secondCell, dir);
        allWalls.Add(wall);
        if (secondCell != null)
        {
           // wall = Instantiate(wallPrefab) as MazeWall;
           wall.Initialize(secondCell, firstCell, dir.GetOpposite());
           secondCell.wallCount++;
            //innerWalls.Add(wall);
        } else
        {
            wall.secondCellIndex = -1;
        }
        
    }

    private MazeCell CreateCell(MyVector2D coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        newCell.myMaze = this;
        newCell.nmsurface = newCell.GetComponentInChildren<NavMeshSurface>();
        return newCell;
    }
}
