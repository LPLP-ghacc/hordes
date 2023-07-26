using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCGen : MonoBehaviour
{
    [Header("Createble Prefabs")]
    public List<GameObject> CreatableWalls = new List<GameObject>();

    [Header("Generation Settings")]
    public int mapLength = 1000;
    public int intensity;
    public float spawnOffset;

    [Range(0f, 10f)]
    public float maxRandomValue;

    private List<GameObject> CreatedWalls = new List<GameObject>();

    private protected void Start()
    {
        var parent = transform.Find("CreatedWalls");

        CreateWalls(parent);

        Debug.Log(CreatableWalls.Count);
    }

    private void CreateWalls(Transform parent)
    {
        var startpos = ((mapLength / 2) * -1);

        int x = 0;

        for (int j = 0; j < mapLength; j++)
        {
            int z = 0;

            var posx = mapLength / intensity;
            x += posx;

            if (x >= 1000)
                return;

            for (int i = 0; i < intensity; i++)
            {
                var posz = mapLength / intensity;
                z += posz;

                var wall = Instantiate(CreatableWalls[Random.Range(0, CreatableWalls.Count)], parent);

                wall.transform.localPosition = new Vector3((startpos + x) + Random.Range(0, maxRandomValue), spawnOffset, (startpos + z) + Random.Range(0, maxRandomValue));

                wall.transform.localRotation = Random.value > .5f ? Quaternion.Euler(0f, 90f, 0) : Quaternion.identity;

                CreatedWalls.Add(wall);
            }
        }
    }
}
