using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int size_x;
    public int size_z;

    public Cell cellPrefab;
    private Cell[,] cells;
    private List<Cell> maze;
    private List<Cell> frontier;

    private void Awake()
    {
        maze = new List<Cell>();
        frontier = new List<Cell>();
    }
    public void Generate()
    {
        cells = new Cell[size_x, size_z];
        for (int x = 0; x < size_x; x++)
        {
            for (int z = 0; z < size_z; z++)
            {
                CreateCell(x, z);
            }
        }
    }

    void CreateCell(int x, int z)
    {
        Cell newCell = Instantiate(cellPrefab) as Cell;
        cells[x, z] = newCell;
        newCell.name = "Maze cell " + x + ", " + z;
        newCell.transform.parent = transform;
        // the vector3 position below works in a way that makes maze center = 0,0,0
        newCell.transform.localPosition = new Vector3(x - size_x * 0.5f + 0.5f, 0f, z - size_z * 0.5f + 0.5f);
        newCell.weight = Random.Range(1, 9);
        newCell.textPrefab.text = newCell.weight.ToString();

        // SETTING NEIGHBORS 
        if (x > 0)
        {
            newCell.SetNeighbor(cells[x - 1, z], 3);
            newCell.createWall(WallDirections.W);
        }
        if (z > 0)
        {
            newCell.SetNeighbor(cells[x, z - 1], 2);
            newCell.createWall(WallDirections.S);
        }
        if (x == 0)
        {
            newCell.createWall(WallDirections.W);
        }
        if (x == size_x - 1)
        {
            newCell.createWall(WallDirections.E);
        }
        if (z == 0)
        {
            newCell.createWall(WallDirections.S);
        }
        if (z == size_z - 1)
        {
            newCell.createWall(WallDirections.N);
        }
        
    }

    public void GenerateMaze()
    {
        // Starting off with a random cell.
        Cell start = cells[Random.Range(0, size_x), Random.Range(0, size_z)];
        maze.Add(start);
        // Adding any neighbors of the cell we started with to the frontier list.
        for (int i = 0; i < 4; i++)
        {
            if (start.GetNeighbor(i) != null)
            {
                frontier.Add(start.GetNeighbor(i));
            }
        }

        while (frontier.Count > 0)
        {

            int index = Random.Range(0, frontier.Count - 1);
            List<Cell> possibleChoices = new List<Cell>();

            for (int i = 0; i < 4; i++)
            {
                if (frontier[index].GetNeighbor(i) != null && maze.Contains(frontier[index].GetNeighbor(i)))
                {
                    possibleChoices.Add(frontier[index].GetNeighbor(i));
                }
            }

            Cell nextChoice = possibleChoices[Random.Range(0, possibleChoices.Count - 1)];
            WallDirections wallToDelete = Cell.whichNeighbor(nextChoice, maze[maze.Count - 1]);
            Destroy(maze[maze.Count - 1].GetWall(wallToDelete).gameObject);
            maze.Add(nextChoice);
            frontier.Remove(nextChoice);
            
            
            for (int j = 0; j < 4; j++)
            {
                if (maze[maze.Count - 1].GetNeighbor(j) != null && !maze.Contains(maze[maze.Count - 1].GetNeighbor(j)))
                {
                    frontier.Add(maze[maze.Count - 1].GetNeighbor(j));
                }
            }

            //if (frontier[index].GetNeighbor(i) != null && maze.Contains(frontier[index].GetNeighbor(i)))
            //{
            //    maze.Add(frontier[index]);
            //    frontier.RemoveAt(index);
            //    Destroy(maze[maze.Count].GetWall(i).gameObject);
            //    for (int j = 0; j < 4; j++)
            //    {
            //        if (maze[maze.Count].GetNeighbor(j) != null && !maze.Contains(maze[maze.Count].GetNeighbor(j)))
            //        {
            //            frontier.Add(maze[maze.Count].GetNeighbor(j));
            //        }
            //    }
            //}
        }
    }

}
