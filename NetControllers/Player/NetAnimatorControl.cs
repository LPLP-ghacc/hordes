using Assets._NETWORK;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetAnimatorControl : MonoBehaviour
{
    public List<Collider> rigidColliders = new List<Collider>();

    private NetworkingPlayerController controller;
    private Rigidbody _rigi;
    private Animator _animator;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();

        SetCollidersActive(false);

        if (!_view.IsMine)
            return;

        controller = GetComponent<NetworkingPlayerController>();
        _rigi = GetComponent<Rigidbody>();
        _animator = transform.Find("[PIVOT]/[MESH]").GetComponent<Animator>();

        if(_rigi == null || _animator == null)
        {
            Debug.Log("_rigi or _animator null. NetAnimatorControl.");
            return;
        }
    }

    private void SetCollidersActive(bool value)
    {
        foreach (var collider in rigidColliders)
        {
            collider.enabled = value;

            var rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody != null) rigidbody.isKinematic = !value;
        }
    }

    private void Update()
    {
        if(!_view.IsMine)
            return;

        bool isGrounded = controller.isGrounded;

        _animator.SetFloat("speed", (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * _rigi.velocity.magnitude));

        _animator.SetFloat("strafe", (float)(0.5 * (Input.GetAxis("Horizontal") * 5)));

        _animator.SetBool("ground", isGrounded);

        _animator.SetBool("crouch", controller.isCrouched);

        if (Input.GetKeyDown(HorDesKeys.jump) && isGrounded)
        {
            _animator.SetTrigger("jump");
        }
    }
}
