using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Maze : MonoBehaviour {
    public MyVector2D size;
    public MazeCell cellPrefab;
    private MazeCell[,] cells;
    public float generationStopDelay;

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

    public IEnumerator GenerateCells()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStopDelay);
        cells = new MazeCell[size.x, size.z];
        MyVector2D coordinates = RandomCoordinates;
        while (ContainsCoordinates(coordinates))
        {
            yield return delay;
            CreateCell(coordinates);
            coordinates.z += 1;
        }
    }

    private void CreateCell(MyVector2D coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
    }
}
