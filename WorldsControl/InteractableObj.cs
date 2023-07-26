using Assets;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObj : MonoBehaviour
{
    public string LoadLevel;

    public bool isJumpTrigger = false;

    private void OnTriggerStay(Collider other)
    {
        if (!isJumpTrigger)
        {
            PhotonView photonPlayer = (other.gameObject) ? other.gameObject.GetComponent<PhotonView>() : null;

            if (photonPlayer && photonPlayer.IsMine)
            {
                NetworkingPlayerController controller = photonPlayer.GetComponent<NetworkingPlayerController>();

                if (controller && controller.isInteract)
                    OpenDoor(); StartCoroutine(LoadWorld(1f));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isJumpTrigger) 
        {
            var obj = other.gameObject;
            var controller = (obj.GetComponent<NetworkingPlayerController>()) ? obj.GetComponent<NetworkingPlayerController>() : null;
            var photonPlayer = (other.GetComponent<PhotonView>()) ? other.GetComponent<PhotonView>() : null;

            if (controller)
                StartCoroutine(LoadWorld(0));
            else
                return;
        }
    }

    IEnumerator LoadWorld(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (var pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (pl.GetComponent<NetworkingPlayerController>() && pl.GetComponent<NetworkingPlayerController>().isSelf)
            {
                if (pl.GetComponent<PhotonView>().IsMine)
                {

                    PhotonNetwork.Destroy(pl);

                    SceneManager.LoadScene(LoadLevel);

                }
            }
        }         
    }

    private void OpenDoor()
    {
        var animator = transform.parent.GetComponent<Animator>();

        animator.SetTrigger("open");
    }
}
