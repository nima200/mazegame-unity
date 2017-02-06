using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public Wall wallPrefab;
    public Wall[] myWalls = new Wall[4];
    [SerializeField]
    Cell[] neighbors = new Cell[4];
    public int weight;
    public TextMesh textPrefab;

    public void createWall(WallDirections dir)
    {
        if (this.myWalls[(int)dir] == null)
        {
            Wall wall = Instantiate(wallPrefab) as Wall;
            wall.transform.parent = transform;
            wall.transform.localPosition = WallMetrics.positions[(int)dir];
            wall.transform.localRotation = WallMetrics.rotations[(int)dir];
            myWalls[(int)dir] = wall;

            // SHARING WALLS BETWEEN CELLS
            if ((int)dir < 2)
            {
                if (this.neighbors[(int)dir] != null)
                {
                    if (this.neighbors[(int)dir].GetWall((int)dir + 2) == null)
                        this.neighbors[(int)dir].SetWall(wall, (int)dir + 2);
                }
            }
            else
            {
                if (this.neighbors[(int)dir] != null)
                {
                    if (this.neighbors[(int)dir].GetWall((int)dir - 2) == null)
                    {
                        this.neighbors[(int)dir].SetWall(wall, (int)dir - 2);
                    }
                }
            }
        }
    }
    public Cell GetNeighbor(WallDirections dir)
    {
        return this.neighbors[(int)dir];
    }

    public Cell GetNeighbor(int index)
    {
        return this.neighbors[index];
    }

    public Wall GetWall(WallDirections dir)
    {
        return this.myWalls[(int)dir];
    }

    public Wall GetWall(int index)
    {
        return this.myWalls[index];
    }

    public void SetNeighbor(Cell cell, int dir)
    {
        this.neighbors[dir] = cell;
        if ( dir < 2)
        {
            if (cell.GetNeighbor(dir + 2) == null)
            {
                cell.SetNeighbor(this, dir + 2);
            }
        }
        else
        {
            if (cell.GetNeighbor(dir - 2) == null)
            {
                cell.SetNeighbor(this, dir - 2);
            }
        }
    }

    public void SetWall(Wall wall, int dir)
    {
        this.myWalls[dir] = wall;
    }

    public bool areNeighbors (Cell cell1, Cell cell2)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cell1.GetNeighbor(i) == cell2)
            {
                return true;
            }
        }
        return false;
    }

    public static WallDirections whichNeighbor (Cell cell1, Cell cell2)
    {
        if (cell1.GetNeighbor(0) == cell2)
        {
            return WallDirections.N;
        } else if (cell1.GetNeighbor(1) == cell2)
        {
            return WallDirections.E;
        } else if (cell1.GetNeighbor(2) == cell2)
        {
            return WallDirections.S;
        } else
        {
            return WallDirections.W;
        }
    }
}
