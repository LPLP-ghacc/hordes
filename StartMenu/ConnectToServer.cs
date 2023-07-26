using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(2);


        if (PhotonNetwork.ConnectUsingSettings())
        {
            Debug.Log($"Connected to {PhotonNetwork.Server}");
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Menu");
    }
}
