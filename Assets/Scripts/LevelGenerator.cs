using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // singleton
    private static LevelGenerator _instance;
    public static LevelGenerator Instance { get { return _instance; } }

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _p1Prefab;
    [SerializeField] private GameObject _p2Prefab;

    private int _gridCount = 3;
    private float _cellSize = 0.2f;
    private float _cellSpacing = 0.04f;
    private int _cellCounter = 0;
    public float _offset = 0.24f;

    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void BuildLevel()
    {
        StartCoroutine(BuildGrid());
    }

    private IEnumerator BuildGrid()
    {
        Cell[,,] grid = new Cell[_gridCount, _gridCount, _gridCount];

        // Generate a grid of cells that is _gridSize x _gridSize x _gridSize
        for (int x = 0; x < _gridCount; x++)
        {
            for (int y = 0; y < _gridCount; y++)
            {
                for (int z = 0; z < _gridCount; z++)
                {   
                    Vector3 position = new Vector3((x * _cellSize + x * _cellSpacing), (y *_cellSize + y * _cellSpacing), (z * _cellSize + z * _cellSpacing));
                    Vector3 worldPos = transform.TransformPoint(position);
                    GameObject cellObj = Instantiate(_cellPrefab, worldPos, Quaternion.identity);
                    cellObj.transform.parent = this.transform;

                    cellObj.name = "Cell " + _cellCounter.ToString("D2");

                    Cell cell = cellObj.GetComponent<Cell>();
                    grid[x, y, z] = cell;

                    _cellCounter++;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        // Set neighbors for each cell
        for (int x = 0; x < _gridCount; x++)
        {
            for (int y = 0; y < _gridCount; y++)
            {
                for (int z = 0; z < _gridCount; z++)
                {
                    Cell cell = grid[x, y, z];
                    if (x > 0) cell._neighbors["Left"] = grid[x - 1, y, z];
                    if (x < _gridCount - 1) cell._neighbors["Right"] = grid[x + 1, y, z];
                    if (y > 0) cell._neighbors["Down"] = grid[x, y - 1, z];
                    if (y < _gridCount - 1) cell._neighbors["Up"] = grid[x, y + 1, z];
                    if (z > 0) cell._neighbors["Backward"] = grid[x, y, z - 1];
                    if (z < _gridCount - 1) cell._neighbors["Forward"] = grid[x, y, z + 1];
                }
            }
        }

        StartCoroutine(BuildWarnings());
    }

    private List<Cell> GetCells()
    {
        var cells = new List<Cell>();
        foreach (Transform child in transform)
        {
            cells.Add(child.GetComponent<Cell>());
        }

        return cells;
    }

    private IEnumerator BuildWarnings()
    {
        var cells = GetCells();
        
        foreach (Cell cell in cells)
        {
            Dictionary<string, Vector3> directions = new Dictionary<string, Vector3>
            {
                { "Up", new Vector3(0, _offset, 0) },
                { "Down", new Vector3(0, -_offset, 0) },
                { "Left", new Vector3(-_offset, 0, 0) },
                { "Right", new Vector3(_offset, 0, 0) },
                { "Forward", new Vector3(0, 0, _offset) },
                { "Backward", new Vector3(0, 0, -_offset) }
            };

            foreach (var direction in directions.Keys.ToList())
            {
                if (cell._neighbors.ContainsKey(direction))
                {
                    directions.Remove(direction);
                }
            }
            
            foreach (var direction in directions)
            {
                Vector3 position = cell.transform.position + direction.Value;
                GameObject warning = Instantiate(_warningPrefab, position, Quaternion.identity);
                warning.SetActive(false);
                warning.transform.parent = cell.transform;
                warning.transform.LookAt(cell.transform);
                yield return null;
            }
        }

        BuildPlayers();
    }

    private void BuildPlayers()
    {
        GameObject p1Space = gameObject.transform.GetChild(12).gameObject;
        GameObject p2Space = gameObject.transform.GetChild(14).gameObject;

        GameObject p1 = Instantiate(_p1Prefab, p1Space.transform.position, Quaternion.identity);
        GameObject p2 = Instantiate(_p2Prefab, p2Space.transform.position, Quaternion.identity);
    }

    // Return all cells in the grid
    public List<Cell> GetAllCells()
    {
        return GetCells();
    }
}
