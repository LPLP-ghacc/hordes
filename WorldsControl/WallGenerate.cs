using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

public class WallGenerate : MonoBehaviour
{
    [Header("Generation")]
    public int clusterId;
    public GameObject[] wallVariants;
    public int intensity = 30;
    public int spawnRange = 50;
    public float spawnHeightOffset = 0;
    public bool isGenerationComplete = false;

    [Header("Generation of modified room")]
    public int randomModified = 25;
    public bool isModified = false;
    public GameObject[] modifiedFloor;
    public GameObject[] modifiedCeiling;

    [Header("Combiner")]
    public bool isCombine = true;
    public bool createMultiMaterialMesh = true;
    public bool destroyCombinedChildren = true;
    public bool generateUVMap = true;

    private Transform parent;
    private NavMeshSurface surface;
    private System.Random random;

    protected void Start()
    {
        random = new System.Random(clusterId);

        parent = transform.Find("CreatedWalls");

        isModified = random.Next(0, randomModified) == 1;

        if (!isModified)
            StartCoroutine(StartGenerateWalls(isCombine));
        else
            StartGenerateModifiedRoom();

        BakeNewNavMesh();
    }

    private void BakeNewNavMesh()
    {
        // BAKERYAMAKER
        surface = GameObject.Find("BakerYaMaker").GetComponent<NavMeshSurface>();

        surface.BuildNavMesh();
    }

    private void StartGenerateModifiedRoom()
    {
        if(random.Next(0, 2) == 0)
        {
            if (modifiedFloor.Length != 0)
            {
                var defaultFloor = transform.Find("Floor");
                Destroy(defaultFloor.gameObject);
                Instantiate(modifiedFloor[random.Next(0, modifiedFloor.Length)], transform.parent.transform.position, Quaternion.identity, transform);
            }
        }
        else
        {
            if (modifiedCeiling.Length != 0)
            {
                var defaultCeiling = transform.Find("Сeiling");

                Destroy(defaultCeiling.gameObject);

                Instantiate(modifiedCeiling[random.Next(0, modifiedCeiling.Length)], transform.parent.transform.position + new Vector3(0, 5.5f, 0), Quaternion.Euler(180, 0, 0), transform);
            }
        }
    }

    private IEnumerator StartGenerateWalls(bool isCombine)
    {
        List<GameObject> walls = new List<GameObject>();

        for(int i = 0; i < intensity; i++)
        {
            yield return null;

            var localRotation = (random.Next(0, 1) == 1) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 90, 0);

            var wall = Instantiate(wallVariants[random.Next(0, wallVariants.Length)], Vector3.zero, localRotation, parent);

            wall.transform.localPosition = new Vector3(random.Next(-spawnRange, spawnRange), spawnHeightOffset, random.Next(-spawnRange, spawnRange));

            if(!isCombine)
            {
                var collider = wall.AddComponent<MeshCollider>();

                collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
            }

            walls.Add(wall);
        }

        if (isCombine)
        {
            var combiner = parent.gameObject.AddComponent<MeshCombiner>();

            combiner.CreateMultiMaterialMesh = createMultiMaterialMesh;
            combiner.DestroyCombinedChildren = destroyCombinedChildren;
            combiner.GenerateUVMap = generateUVMap;
            combiner.CombineMeshes(true);

            var collider = parent.gameObject.AddComponent<MeshCollider>();
            collider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        }
        parent.gameObject.tag = "Wall";

        isGenerationComplete = true;
    }
}
