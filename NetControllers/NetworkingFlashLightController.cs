using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingFlashLightController : MonoBehaviour
{
    public KeyCode enableKey = KeyCode.F;

    private PhotonView _player;

    private Animator _animator;

    public bool enable = false;

    private void Start()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkingPlayerController>() && player.GetComponent<NetworkingPlayerController>().isSelf)
            {
                _player = player.GetComponent<PhotonView>();
            }
        }

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_player != null && _player.IsMine)
        {
            if(Input.GetKeyDown(enableKey))
            {
                if(enable)
                {
                    enable = false;
                    _animator.SetBool("idle", enable);
                    return;
                }else
                {
                    enable = true;
                    _animator.SetBool("idle", enable);
                    return;
                }
            }
        }   
    }
}
