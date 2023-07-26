using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingPlayerPresense : MonoBehaviour
{
    public string lampName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.StartsWith(lampName))
        {
            other.gameObject.GetComponent<Light>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith(lampName))
        {
            other.gameObject.GetComponent<Light>().enabled = false;
        }
    }
}
