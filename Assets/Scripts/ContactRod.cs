using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactRod : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ContactRod"))
        {
            DangerManager.Instance._rodsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ContactRod"))
        {
            DangerManager.Instance._rodsOverlapping = false;
        }
    }
}
