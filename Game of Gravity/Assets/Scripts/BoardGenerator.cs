using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public int boardSize = 5; // default board size is 5x5
    public GameObject cellPrefab;
    public GameObject linePrefab;

    void Start()
    {
        // Generate the cells of the board
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                GameObject cell = Instantiate(cellPrefab, transform.Find("Cells"));
                cell.name = "Cell (" + row + ", " + col + ")";
                cell.transform.position = new Vector3(col - (boardSize - 1) / 2f, row - (boardSize - 1) / 2f, 0);
                cell.AddComponent<BoxCollider2D>();

                // Add line renderers between cells
                if (col > 0)
                {
                    GameObject line = Instantiate(linePrefab, transform.Find("Lines"));
                    line.transform.position = new Vector3((col - 1) - (boardSize - 1) / 2f, row - (boardSize - 1) / 2f, 0);
                }
                if (row > 0)
                {
                    GameObject line = Instantiate(linePrefab, transform.Find("Lines"));
                    line.transform.position = new Vector3(col - (boardSize - 1) / 2f, (row - 1) - (boardSize - 1) / 2f, 0);
                    line.transform.Rotate(0, 0, 90);
                }
            }
        }
    }
}
