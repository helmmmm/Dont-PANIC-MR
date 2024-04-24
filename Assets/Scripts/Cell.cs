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
            if (other != this)
                IsOccupied = true;
            else return;
            //Debug.Log($"Occupied on Cell: {gameObject.name} \n");
        }
            
        if (other.CompareTag("ContactRod"))
        {
            IsDanger = true;
            DangerManager.Instance._dangerCellCount++;
            //Debug.Log($"Danger on Cell: {gameObject.name} \n");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            IsOccupied = false;
        if (other.CompareTag("ContactRod"))
            IsDanger = false;
    }
}
