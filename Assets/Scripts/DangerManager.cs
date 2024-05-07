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

    private Player _player1;
    private Player _player2;

    public int _dangerCellCount;

    public void DangerManager_PreRound()
    {
        GenerateWarnings();
    }

    public void DangerManager_MidRound() 
    {
        DisplayWarnings(_randomWarnings);
    }

    public void DangerManager_PostRound()
    {
        DeactivateWarnings();
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

    public void RegisterPlayers(GameObject player1, GameObject player2)
    {
        _player1 = player1.GetComponent<Player>();
        _player2 = player2.GetComponent<Player>();
    }

    public void DeactivateWarnings()
    {
        foreach (GameObject warning in _randomWarnings)
        {
            warning.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ClearRound()
    {
        foreach (GameObject warning in _randomWarnings)
        {
            warning.transform.GetChild(0).gameObject.SetActive(false);
            _dangerCells.Clear();
            _dangerCellCount = 0;
        }
    }

    public void ShootLasers()
    {
        StartCoroutine(ActivateLasers());
    }
    
    private IEnumerator ActivateLasers()
    {
        _player1._movable = false;
        _player2._movable = false;

        if (_randomWarnings.Count < 1)
            yield return null;

        foreach (GameObject warning in _randomWarnings)
        {
            warning.transform.GetChild(0).gameObject.SetActive(false);
            warning.transform.GetChild(1).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject warning in _randomWarnings)
        {
            warning.transform.GetChild(1).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
        
        if (!GameManager.Instance.IsGameOver)
            Game_SM.Instance.TryChangeState(Game_SM.Instance.GameState_PostRound);
        else
            Game_SM.Instance.TryChangeState(Game_SM.Instance.GameState_GameOver);
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

    public List<GameObject> RegisterWarnings(List<Cell> allCells)
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
        var allWarningsCopy = new List<GameObject>(_allWarnings);
        _randomWarnings.Clear();

        for (int i = 0; i < dangerCount; i++)
        {
            if (allWarningsCopy.Count == 0)
            {
                break;
            }

            var randomIndex = Random.Range(0, allWarningsCopy.Count);
            var randomWarning = allWarningsCopy[randomIndex];

            _randomWarnings.Add(randomWarning);
            allWarningsCopy.RemoveAt(randomIndex);
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
