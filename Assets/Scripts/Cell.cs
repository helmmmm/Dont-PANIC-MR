using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Dictionary<string, Cell> _neighbors = new Dictionary<string, Cell>();
    public bool IsOccupied { get; set; }
    public bool IsDanger { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            IsOccupied = false;
    }
}
