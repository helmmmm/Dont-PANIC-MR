using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelPositioner : MonoBehaviour
{
    public void PositionLevel()
    {
        if (NetworkManager.Singleton.IsServer) // Check if this is the host
        {
            // position level 1m in front of the host
            transform.position = Camera.main.transform.position + new Vector3(-0.25f, -0.6f, 1f);
        }
    }
}
