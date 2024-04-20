using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _p1Prefab;
    [SerializeField] private GameObject _p2Prefab;


    private int _gridCount = 3;
    private float _cellSize = 0.2f;
    private float _cellSpacing = 0.04f;
    private int _cellCounter = 0;
    public float _offset = 0.24f;

    public void BuildLevel()
    {
        StartCoroutine(BuildGrid());
    }

    private IEnumerator BuildGrid()
    {
        // Generate a grid of cells that is _gridSize x _gridSize x _gridSize
        for (int x = 0; x < _gridCount; x++)
        {
            for (int y = 0; y < _gridCount; y++)
            {
                for (int z = 0; z < _gridCount; z++)
                {   
                    Vector3 position = new Vector3((x * _cellSize + x * _cellSpacing), (y *_cellSize + y * _cellSpacing), (z * _cellSize + z * _cellSpacing));
                    Vector3 worldPos = transform.TransformPoint(position);
                    GameObject cell = Instantiate(_cellPrefab, worldPos, Quaternion.identity);
                    cell.transform.parent = this.transform;

                    cell.name = "Cell " + _cellCounter.ToString("D2");

                    _cellCounter++;
                    yield return new WaitForSeconds(0.05f);
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

            foreach (Cell neighbor in cell._neighborCells)
            {
                if (neighbor.transform.position.x > cell.transform.position.x)
                {
                    directions.Remove("Right");
                }
                if (neighbor.transform.position.x < cell.transform.position.x)
                {
                    directions.Remove("Left");
                }
                if (neighbor.transform.position.y > cell.transform.position.y)
                {
                    directions.Remove("Up");
                }
                if (neighbor.transform.position.y < cell.transform.position.y)
                {
                    directions.Remove("Down");
                }
                if (neighbor.transform.position.z > cell.transform.position.z)
                {
                    directions.Remove("Forward");
                }
                if (neighbor.transform.position.z < cell.transform.position.z)
                {
                    directions.Remove("Backward");
                }
            }
            
            foreach (var direction in directions)
            {
                Vector3 position = cell.transform.position + direction.Value;
                GameObject warning = Instantiate(_warningPrefab, position, Quaternion.identity);
                warning.transform.parent = cell.transform;
                //yield return new WaitForSeconds(0.01f);
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
}
