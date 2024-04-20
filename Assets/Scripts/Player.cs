using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayer1;
    private LevelGenerator _levelGenerator;
    private Cell _currentCell;

    private void Start()
    {
        _levelGenerator = GameObject.Find("Level Space").GetComponent<LevelGenerator>();
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
            if (isPlayer1)
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
        Vector3 currentPos = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(currentPos, destination - currentPos, out hit, 1f))
        {
            if (hit.collider.CompareTag("Cell"))
            {
                return !hit.collider.GetComponent<Cell>().IsOccupied;
            }
        }
        
        return false;
    }
}
