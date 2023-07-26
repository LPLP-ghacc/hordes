using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVariant : MonoBehaviour
{
    public GameObject[] variant;

    private int seed;

    void Start()
    {
        if(GameObject.Find("GameController") != null)
        {
            seed = GameObject.Find("GameController").GetComponent<Maze_GC>().seed;
        }

        Random.InitState(seed);

        variant[Random.Range(0, variant.Length)].SetActive(true);
    }
}
