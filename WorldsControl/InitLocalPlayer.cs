using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class InitLocalPlayer : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    public void CreateLocalPlayer()
    {
        var player = SpawnPlayers.GetPlayerFromPool();

        if (spawnPoints.Count == 0)
        {
            PhotonNetwork.Instantiate(player, Vector3.zero, Quaternion.identity);
        } else
        {
            var point = spawnPoints[Random.Range(0, spawnPoints.Count)];

            PhotonNetwork.Instantiate(player, point.position, point.rotation);
        }
    }

    public void CreatePlayerFromCoords(Vector3 pos)
    {
        var player = SpawnPlayers.GetPlayerFromPool();

        PhotonNetwork.Instantiate(player, pos, Quaternion.identity);
    }
}
