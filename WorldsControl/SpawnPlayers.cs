using Photon.Pun;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPlayers : MonoBehaviour
{
    public float waitForSeconds = 1;
    public GameObject[] _NETWORKPLAYER;
    public Transform[] spawnPoints;

    private void Start()
    {
        StartCoroutine(OnStart());
    }

    private IEnumerator OnStart()
    {
        yield return new WaitForSeconds(waitForSeconds);

        //Get Player (string)
        string player = GetPlayerFromPool();

        //Spawn Player
        if (spawnPoints.Length > 0)
        {
            PhotonNetwork.Instantiate(player, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        }
        else
        {
            var mapRange = GameObject.Find("_WORLD").GetComponent<GameController>().mapRange;

            var spawnRange = mapRange * 100;

            PhotonNetwork.Instantiate(player, new Vector3(Random.Range(100, spawnRange - 100), 0, Random.Range(100, spawnRange - 100)), Quaternion.identity);
        }
    } 

    public static string GetPlayerFromPool()
    {
        var pool = LocalRes.ReturnLocalRes().pool;

        string player = "";

        foreach (var p in pool.ResourceCache)
        {
            if (p.Value.GetComponent<NetworkingPlayerController>())
            {
                player = p.Key;
                break;
            }
        }

        return player;
    }
}
