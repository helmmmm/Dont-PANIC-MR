using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public List<Cell> _neighborCells = new List<Cell>();
    public bool IsOccupied { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cell"))
            _neighborCells.Add(other.GetComponent<Cell>());
        if (other.CompareTag("Player"))
            IsOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            IsOccupied = false;
    }

    public List<Cell> GetNeighborsCells()
    {
        return _neighborCells;
    }
}
