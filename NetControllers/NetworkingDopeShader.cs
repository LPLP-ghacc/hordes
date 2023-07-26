using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingDopeShader : MonoBehaviour
{
    private NetworkingPlayerController _controller;

    void Start()
    {
        _controller = this.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<NetworkingPlayerController>();
    }

    void Update()
    {
        
    }
}
