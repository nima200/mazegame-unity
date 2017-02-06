using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {
    public MazeCell firstCell;
    public MazeCell secondCell;
    public MazeDirection direction;
    public int firstCellIndex;
    public int secondCellIndex;
    

    public void Initialize (MazeCell firstCell, MazeCell secondCell, MazeDirection direction)
    {
        this.firstCell = firstCell;
        this.secondCell = secondCell;
        this.direction = direction;
        this.firstCellIndex = (int)direction;
        this.secondCellIndex = (int)direction.GetOpposite();
        firstCell.SetEdge(direction, this);
        transform.parent = firstCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }

    public void destroy()
    {

    }
}
