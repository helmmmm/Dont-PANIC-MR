using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerViewPositioner : NetworkBehaviour
{
    [SerializeField] private GameObject _levelSpaceOrigin;

    public void PositionPlayer()
    {
        float distanceFromLevel = LevelGenerator.Instance.LevelDepth / 2 - 1f;
        
        if (IsHost)
        {
            transform.position = Vector3.zero - new Vector3(0, 0, distanceFromLevel);
            Debug.Log("Host Repositioned!");
        }
        else
        {
            transform.position = Vector3.zero + new Vector3(0, 0, distanceFromLevel);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("Client Repositioned!");
        }
    }
}
