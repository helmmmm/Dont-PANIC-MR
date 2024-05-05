using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public List<GameObject> _cellsAffected = new List<GameObject>();
    private LevelGenerator _levelGenerator;

    private void Start()
    {
        _levelGenerator = GameObject.Find("Cells").GetComponent<LevelGenerator>();
        Vector3 warningDirection = transform.forward;

        foreach (Cell cell in _levelGenerator.GetAllCells())
        {
            Vector3 directionToCell = (cell.transform.position - transform.position).normalized;
            
            if (Mathf.Approximately(Vector3.Dot(warningDirection, directionToCell), 1f))
            {
                _cellsAffected.Add(cell.gameObject);
                //Debug.Log("Cell " + cell.name + " is affected by " + this.name);
            }
        }
    }
}
