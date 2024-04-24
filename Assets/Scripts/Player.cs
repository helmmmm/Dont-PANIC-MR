using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool _isPlayer1;
    public bool _movable;
    private LevelGenerator _levelGenerator;
    Game_SM _gameSM => Game_SM.Instance;
    private Cell _currentCell;

    private float[] lastInputTime = new float[6];
    private float _minInputDelay = 0.1f;

    private void Start()
    {
        _levelGenerator = GameObject.Find("Cells").GetComponent<LevelGenerator>();
    }

    private void OnEnable()
    {
        _movable = false;
        _gameSM.GameState_PreGame.OnEnter += DisableMovement;
        _gameSM.GameState_PreRound.OnEnter += DisableMovement;
        _gameSM.GameState_MidRound.OnEnter += EnableMovement;
        _gameSM.GameState_PostRound.OnEnter += DisableMovement;
        _gameSM.GameState_Pause.OnEnter += DisableMovement;
    }

    private void EnableMovement()
    {
        _movable = true;
    }

    private void DisableMovement()
    {
        _movable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            _currentCell = other.GetComponent<Cell>();
        }
    }

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // if any button pressed
        if (Input.anyKeyDown)
        {
            if (Time.realtimeSinceStartup - lastInputTime[0] < _minInputDelay) return;
            if (!_movable) return;
            lastInputTime[0] = Time.realtimeSinceStartup;

            if (_isPlayer1)
            {
                if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector3.forward;
                if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector3.back;
                if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector3.left;
                if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector3.right;
                if (Input.GetKeyDown(KeyCode.V)) moveDirection = Vector3.down;
                if (Input.GetKeyDown(KeyCode.G)) moveDirection = Vector3.up;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) moveDirection = -Vector3.forward;
                if (Input.GetKeyDown(KeyCode.DownArrow)) moveDirection = -Vector3.back;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) moveDirection = -Vector3.left;
                if (Input.GetKeyDown(KeyCode.RightArrow)) moveDirection = -Vector3.right;
                if (Input.GetKeyDown(KeyCode.Keypad1)) moveDirection = Vector3.down;
                if (Input.GetKeyDown(KeyCode.Keypad4)) moveDirection = Vector3.up;
            }

            Vector3 destination = transform.position + moveDirection * _levelGenerator._offset;

            if (IsCellOpenAt(destination))
            {
                transform.position = destination;
            }
        }
    }

    private bool IsCellOpenAt (Vector3 destination)
    {
        Cell cell = GetCellAt(destination);
        if (cell == null || cell.IsOccupied)
        {
            return false;
        }

        float distance = Vector3.Distance(destination, cell.transform.position);
        float threshold = 0.1f;
        if (distance < threshold)
        {
            return true;
        }

        return false;
        //destination = new Vector3(Mathf.Round(destination.x), Mathf.Round(destination.y), Mathf.Round(destination.z));

        //Vector3 currentPos = transform.position;
        //RaycastHit hit;
        //if (Physics.Raycast(currentPos, destination - currentPos, out hit, 1f))
        //{
        //    if (hit.collider.CompareTag("Cell"))
        //    {
        //        return !hit.collider.GetComponent<Cell>().IsOccupied;
        //    }
        //}
        
        //return false;
    }

    private Cell GetCellAt(Vector3 position)
    {
        position = new Vector3(Mathf.Round(position.x * 100) / 100.0f, 
                                Mathf.Round(position.y * 100) / 100.0f,
                                Mathf.Round(position.z * 100) / 100.0f);
        var allCells = _levelGenerator.GetAllCells();
        foreach (Cell cell in allCells)
        {
            Vector3 cellPos = new Vector3(Mathf.Round(cell.transform.position.x * 100) / 100.0f, 
                                            Mathf.Round(cell.transform.position.y * 100) / 100.0f,
                                            Mathf.Round(cell.transform.position.z * 100) / 100.0f);
            if (cellPos == position)
            {
                return cell;
            }
        }
        return null;
    }

    private void OnDisable()
    {
        _gameSM.GameState_PreGame.OnEnter -= DisableMovement;
        _gameSM.GameState_PreRound.OnEnter -= DisableMovement;
        _gameSM.GameState_MidRound.OnEnter -= EnableMovement;
        _gameSM.GameState_PostRound.OnEnter -= DisableMovement;
        _gameSM.GameState_Pause.OnEnter -= DisableMovement;
    }
}
