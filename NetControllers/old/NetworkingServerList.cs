using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkingServerList : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI text;

    public TextMeshProUGUI serverList;

    void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = $"Online players: {PhotonNetwork.CountOfPlayers}";
    }

    void Update()
    {

    }
}
