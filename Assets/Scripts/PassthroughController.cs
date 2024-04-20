using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughController : MonoBehaviour
{

    [SerializeField] OVRPassthroughLayer[] _ptLayers;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _ptLayers[0].enabled = !_ptLayers[0].enabled;
        }
    }
}
