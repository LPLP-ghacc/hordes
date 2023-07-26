using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVWallGenerator : MonoBehaviour
{
    public int clusterID;

    public List<GameObject> wallPrefabs = new List<GameObject>();
    [Range(0, 255)]
    public byte size = 100;
    [Range(0, 30)]
    public byte intensity = 10;
    [Range(30, 90)]
    public int randomFailureMaxValue = 9;

    [Space]
    public bool generateUniqueRoom;
    public List<GameObject> uniqueObjects = new List<GameObject>();

    [HideInInspector]
    public bool isGenerationComplete = false;

    [Space]
    public bool meshCombine = false;
    public bool createMultiMaterialMesh;
    public bool destroyCombinedChildren;
    public bool generateUVMap;

    [Space]
    public bool generatePlayerItems;
    [Range(0, 255)]
    public byte itemsCoincidence; //3
    [Range(0, 255)]
    public byte itemsCount;
    public List<GameObject> items = new List<GameObject>();

    [Space]
    public bool generateEnemies;
    [Range(0, 255)]
    public byte enemyCoincidence; // 10
    public List<GameObject> enemies = new List<GameObject>();

    private System.Random random;
    private MeshCombiner combiner;

    public void StartGenerate()
    {
        random = new System.Random(clusterID);


        generateUniqueRoom = random.Next(0, randomFailureMaxValue) == 0;

        combiner = GetComponent<MeshCombiner>();

        generatePlayerItems = random.Next(0, itemsCoincidence) == 0 ? true : false;
        generateEnemies = random.Next(0, enemyCoincidence) == 0 ? true : false;

        if (!generateUniqueRoom)
        {
            StartCoroutine(InitWallsCoroutine());
            StartCoroutine(GeneratePlayerItems(generatePlayerItems));
            GenerateEnemies(generateEnemies);
        }else
        {
            Instantiate(uniqueObjects[random.Next(0, uniqueObjects.Count)], new Vector3(size / 2, size / 2), Quaternion.identity, transform);
        }

        gameObject.tag = "Wall";
        isGenerationComplete = true;
    }

    private IEnumerator InitWallsCoroutine()
    {
        yield return null;

        List<Vector3> wallPositions = new List<Vector3>();

        int step = size / intensity;

        //Get positions of walls
        for (int i = 1; i <= intensity; i++)
        {
            int z = i * step;

            for (int j = 0; j < intensity; j++)
            {
                int x = j * step;

                wallPositions.Add(new Vector3(x, transform.position.y, z));
            }
        }

        int wallPrefabsCount = wallPrefabs.Count;

        //Instantiate wall by position
        for (int k = 0; k < wallPositions.Count; k++)
        {
            yield return null;

            if (wallPrefabsCount == 0)
                continue;

            GameObject wallPrefab = wallPrefabs[random.Next(0, wallPrefabsCount)];

            GameObject wall = Instantiate(wallPrefab, transform);

            //Rotate current wall by random
            if (random.Next(0, 2) == 0)
                wall.transform.rotation = Quaternion.Euler(0, 90, 0);

            //it is worth adding some deviation, so that there is no overrender
            var deviation = new Vector3(random.Next(0, 5) / 10, 0, random.Next(0, 5) / 10);

            wall.transform.localPosition = wallPositions[k] + deviation;
        }

        if (meshCombine && combiner != null)
        {
            combiner.CreateMultiMaterialMesh = createMultiMaterialMesh;
            combiner.DestroyCombinedChildren = destroyCombinedChildren;
            combiner.GenerateUVMap = generateUVMap;
            combiner.CombineMeshes(true);

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;

            Destroy(combiner);
        }
    }

    private IEnumerator GeneratePlayerItems(bool generateItems)
    {
        if(generateItems && items.Count != 0)
        {
            List<Vector3> itemsPositions = new List<Vector3>();

            for (int i = 0; i < itemsCount; i++)
            {
                itemsPositions.Add(new Vector3(random.Next(0, size), 0.5f, random.Next(0, size)));
            }

            //Generate Items
            for (int i = 0; i < itemsPositions.Count; i++)
            {
                yield return null;
                //set it for online game
                Instantiate(items[random.Next(0, items.Count)], itemsPositions[i], Quaternion.identity, transform);
            }
        }
    }

    private void GenerateEnemies(bool generateEnemies)
    {
        //only one emeny can spawn on cluster

        if (generateEnemies && enemies.Count != 0)
        {
            var position = new Vector3(random.Next(0, size), 0.5f, random.Next(0, size));

            Instantiate(enemies[random.Next(0, enemies.Count)], position, Quaternion.identity, transform);
        }
    }
}