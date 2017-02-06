using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Maze : MonoBehaviour {
    public MyVector2D size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public List<MazeWall> allWalls = new List<MazeWall>();
    public List<MazeWall> toDelete = new List<MazeWall>();
    //public List<MazeWall> innerWalls = new List<MazeWall>();
    public float generationStopDelay;
    public int alcoveCount = 0;

    public MyVector2D RandomCoordinates {
        get
        {
            return new MyVector2D(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public void killDeadEnds()
    {
        
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

    }

    void colorAlcoves()
    {

        foreach(MazeCell cell in cells)
        {
            if (cell.wallCount == 3)
            {
                cell.GetComponentInChildren<Renderer>().material.color = Color.black;
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
        for (int i = 0; i < allWalls.Count; i++)
        {
            //if (alcoveCount < 4)
            //{
            //    break;
            //} else
            //{
                if (allWalls[i].firstCell == null || allWalls[i].secondCell == null)
                {
                    continue;
                }
                else
                {
                    MazeWall wall = allWalls[i];
                    toDelete.Add(wall);
                    if (allWalls[i].firstCell.wallCount == 3 && alcoveCount > 3)
                    {
                        allWalls[i].firstCell.DestroyWall(allWalls[i]);
                    }
                    if (allWalls[i].secondCell.wallCount == 3 && alcoveCount > 3/* && (allWalls[i].firstCell != null && allWalls[i].firstCell.wallCount < 3) */)
                    {
                        allWalls[i].secondCell.DestroyWall(allWalls[i]);
                    }
                    allWalls.Remove(wall);
                    countAlcoves();
                }
            }
        //}
        //deleteExtraAlcoves();
    }
    void deleteExtraAlcoves()
    {
        for (int i = 0; i < toDelete.Count; i++)
        {
            allWalls.Remove(toDelete[i]);
        }
    }
    void PlaceAlcoves()
    {

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
        return newCell;
    }
}
