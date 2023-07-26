using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Generate Settings")]
    public GameObject cluster;
    public int mapRange = 4;
    public int clusterOffset = 100;
    public int seed = 666;
    public System.Random random;

    public List<Vector3> spawnPointOfCluster = new List<Vector3>();

    public InitLocalPlayer initLocalPlayer;

    private void Start()
    {
        StartCoroutine(EnumStart());
    }

    private IEnumerator EnumStart()
    {
        random = new System.Random(seed);

        int counter = 0;

        for (int i = 0; i < mapRange; i++)
        {
            for (int j = 0; j < mapRange; j++)
            {
                yield return null;

                float globalOffSet = mapRange * clusterOffset - mapRange * clusterOffset * .5f;

                var localcluster = Instantiate(cluster, new Vector3(clusterOffset * i - globalOffSet, 0, clusterOffset * j - globalOffSet), Quaternion.identity, transform);

                localcluster.name = $"CLUSTER_{counter}";

                if(random.Next(0, mapRange * mapRange) > 7)
                {
                    localcluster.GetComponent<ClusterController>().seed = random.Next();
                }
                else
                {
                    //this is spawn room
                    localcluster.GetComponent<ClusterController>().isSpawnRoom = true;

                    spawnPointOfCluster.Add(localcluster.transform.position);
                }

                counter++;
            }
        }

        if (spawnPointOfCluster.Count != 0)
        {
            initLocalPlayer.CreatePlayerFromCoords(GetRndPos());
        }
    }

    private Vector3 GetRndPos()
    {
        var currentPos = spawnPointOfCluster[Random.Range(0, spawnPointOfCluster.Count)];

        float rnd = Random.Range(0, 7);

        currentPos += new Vector3(rnd, 0, -rnd);

        Debug.Log(currentPos);

        return currentPos;
    }
}
