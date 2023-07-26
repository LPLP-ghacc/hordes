using Assets._NETWORK;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateController : MonoBehaviourPunCallbacks
{
    public GameObject meshSpineOfPlayer;

    [Space]
    public List<Collider> colliders = new List<Collider>();

    public AudioSource walkSound, runSound, jumpSound;

    private PhotonView view;
    private NetworkingPlayerController controller;
    private Animator animator;
    private GameObject mesh;

    private void Start()
    {
        view = LocalPlayer.GetLocalView();

        if (!view.IsMine)
            return;

        mesh = transform.Find("[PIVOT]/[MESH]")?.gameObject;
        if (mesh == null)
        {
            Debug.LogError("Mesh game object not found!");
            return;
        }

        animator = mesh.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log(animator != null? "Animator component not found!" : "Animator component found.");
            return;
        }

        controller = GetComponent<NetworkingPlayerController>();

        SetCollidersEnabled(false);
    }

    private void Update()
    {
        animator.SetBool("forward_walk", Input.GetKey(HorDesKeys.forward));
        animator.SetBool("left_strafe_walk", Input.GetKey(HorDesKeys.left));
        animator.SetBool("right_strafe_walk", Input.GetKey(HorDesKeys.right));

        animator.SetBool("back_walk", Input.GetKey(HorDesKeys.back));
        animator.SetBool("left_strafe_back_walk", Input.GetKey(HorDesKeys.left) && Input.GetKey(HorDesKeys.back));
        animator.SetBool("right_strafe_back_walk", Input.GetKey(HorDesKeys.right) && Input.GetKey(HorDesKeys.back));

        animator.SetBool("forward_run", Input.GetKey(HorDesKeys.sprint) && Input.GetKey(HorDesKeys.forward));
        animator.SetBool("left_strafe_run", Input.GetKey(HorDesKeys.sprint) && Input.GetKey(HorDesKeys.forward) && Input.GetKey(HorDesKeys.left));
        animator.SetBool("right_strafe_run", Input.GetKey(HorDesKeys.sprint) && Input.GetKey(HorDesKeys.forward) && Input.GetKey(HorDesKeys.right));

        if (Input.GetKeyDown(HorDesKeys.jump))
        {
            animator.SetBool("jump", Input.GetKeyDown(HorDesKeys.jump));
            jumpSound?.Play();
        }

        if (Input.GetKeyDown(HorDesKeys.forward) && controller.isGrounded)
        {
            walkSound.Play();
        }else
        {
            walkSound?.Stop();
        }

        if (Input.GetKeyDown(HorDesKeys.sprint) && controller.isGrounded)
        {
            runSound.Play();
            walkSound.Stop();
        }else
        {
            runSound.Stop();
        }
        
        animator.SetBool("Grounded", controller.isGrounded);

    }

    public void Kill()
    {
        controller.LocalHide(true);
        SetCollidersEnabled(true);
        controller.enabled = false;
        animator.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
            var rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
                rigidbody.isKinematic = !enabled;
        }
    }
}