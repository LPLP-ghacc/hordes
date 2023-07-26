using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_GC : MonoBehaviour
{
    private static List<GameObject> _networkedPrefabs = new List<GameObject>();

    [Header("HEIGTH OFFSET")]
    public float spawnHeigth = 0f;
    [Header("COMBINE SETTINGS")]
    public bool generateWalls = true;
    public bool generaleLamps = true;
    public bool combine = true;

    [Header("GET SEED FROM PLAYER")]
    public int seed;

    [Header("GET DA PREFABS")]
    public GameObject[] walls;
    public GameObject lamp;

    [Header("GET DA GENERATING SETTINGS")]
    public float maxSpawnDistance = 1000f;
    public int wallSpawnIntensity = 1000;
    public int lampSpawnintensity = 1000;

    private GameObject _wallsSpawnPoint;
    private List<GameObject> _spawnedWalls = new List<GameObject>();

    public List<GameObject> spawnedItems = new List<GameObject>();

    public int spawnedItemsCount = 50;

    public bool isPool = false;

    void Start()
    {
        Random.InitState(seed);

        _wallsSpawnPoint = GameObject.Find("World/Spawn");

        if(generateWalls)
            OnStartCreateWall();

        OnStartCreateItems();     
    }

    private void OnStartCreateItems()
    {
        foreach(var item in _networkedPrefabs)
        {
            if(item.gameObject.name == "Flashlight")
            {
                for (int i = 0; i < spawnedItemsCount; i++)
                {
                    var thisitem = PhotonNetwork.Instantiate(item.gameObject.name, new Vector3(Random.Range(-maxSpawnDistance, maxSpawnDistance), 6f, Random.Range(-maxSpawnDistance, maxSpawnDistance)), Quaternion.identity);

                    thisitem.transform.SetParent(this.gameObject.transform.Find("Item"));
                }
            }
        }
    }

    private void OnStartCreateWall()
    {
        var wallsGameObject = GameObject.Find("GameController/CreatedWalls").gameObject;

        var parent = wallsGameObject.transform;
        var pos = _wallsSpawnPoint.transform.position;

        for (int i = 0; i < wallSpawnIntensity; i++)
        {
            var littleDifference = Random.Range(0f, .00002f);

            var wall = Instantiate(walls[Random.Range(0, walls.Length)], parent);

            wall.transform.localPosition = new Vector3(pos.x + Random.Range(-maxSpawnDistance, maxSpawnDistance) + littleDifference,
                spawnHeigth + littleDifference, pos.z + Random.Range(-maxSpawnDistance, maxSpawnDistance) + littleDifference);

            if (Random.value >= .5f)
                wall.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

            wall.isStatic = true;

            _spawnedWalls.Add(wall);
        }
        if(combine)
        {
            var combiner = wallsGameObject.GetComponent<MeshCombiner>();
            combiner.DestroyCombinedChildren = true;
            combiner.CombineMeshes(true);
            var wallsCollider = wallsGameObject.AddComponent<MeshCollider>();
            wallsCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void PopulateNetworkedPrefabs()
    {
        if (!Application.isEditor)
        {
            return;
        }

        GameObject[] result = Resources.LoadAll<GameObject>("");

        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].GetComponent<PhotonView>() != null)
            {
                _networkedPrefabs.Add(result[i]);
            }
        }
    }
}
