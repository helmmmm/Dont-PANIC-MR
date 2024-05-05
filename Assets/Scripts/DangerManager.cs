using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerManager : MonoBehaviour
{
    public static DangerManager Instance;
    private RoundManager _roundManager;

    private List<GameObject> _randomWarnings = new List<GameObject>();
    private List<Cell> _dangerCells = new List<Cell>();
    private List<Cell> _allCells = new List<Cell>();
    private List<GameObject> _allWarnings = new List<GameObject>();

    public int _dangerCellCount;

    public void DangerManager_PreRound()
    {
        GenerateWarnings();
        DisplayWarnings(_randomWarnings);
    }

    public void DangerManager_MidRound()
    {
        
    }

    public void DangerManager_PostRound()
    {
        ClearWarnings();
    }


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        _roundManager = FindObjectOfType<RoundManager>();
    }

    private void ClearWarnings()
    {
        foreach (GameObject warning in _randomWarnings)
        {
            warning.transform.GetChild(0).gameObject.SetActive(false);
            warning.SetActive(false);
        }
    }   

    private void GenerateWarnings()
    {
        int dangerCount = GetDangerCount();
        int dangerCellCount = 0;
        _allCells = LevelGenerator.Instance.GetAllCells();

        do
        {
            _dangerCellCount = 0;
            _randomWarnings = GetRandomWarnings(dangerCount, _allCells);
            ActivateWarnings(_randomWarnings);
        }
        while (_allCells.Count - dangerCellCount < 2);
    }

    private void DisplayWarnings(List<GameObject> warnings)
    {
        foreach (GameObject warning in warnings)
        {
            warning.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void ActivateWarnings(List<GameObject> warnings)
    {
        foreach (GameObject warning in warnings)
        {
            warning.SetActive(true);
            // access affected cells and make them IsDanger
            foreach (GameObject cell in warning.GetComponent<Warning>()._cellsAffected)
            {
                if (!_dangerCells.Contains(cell.GetComponent<Cell>()))
                {
                    _dangerCells.Add(cell.GetComponent<Cell>());
                    cell.GetComponent<Cell>().IsDanger = true;
                    _dangerCellCount++;
                }
            }
        }
        Debug.Log($"Number of Danger Cells: {_dangerCellCount}");
    }

    private int GetRound()
    {
        return _roundManager._currentRound;
    }

    private List<GameObject> GetAllWarnings(List<Cell> allCells)
    {
        foreach (Cell cell in allCells)
        {
            foreach (Transform warning in cell.transform)
            {
                _allWarnings.Add(warning.gameObject);
            }
        }   
        return _allWarnings;
    }

    private List<GameObject> GetRandomWarnings(int dangerCount, List<Cell> allCells)
    {
        var allWarnings = GetAllWarnings(allCells);

        for (int i = 0; i < dangerCount; i++)
        {
            if (allWarnings.Count == 0)
            {
                break;
            }

            var randomIndex = Random.Range(0, allWarnings.Count);
            var randomWarning = allWarnings[randomIndex];

            _randomWarnings.Add(randomWarning);
            allWarnings.RemoveAt(randomIndex);
        }

        return _randomWarnings;
    }

    private int GetDangerCount()
    {
        var round = GetRound();

        if (round < 5)
        {
            return 4;
        }
        else if (round < 10)
        {
            return 6;
        }
        else if (round < 15)
        {
            return 8;
        }
        else
        {
            return 10;
        }
    }

}
