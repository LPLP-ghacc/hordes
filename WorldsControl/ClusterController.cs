using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClusterController : MonoBehaviour
{
    [Header("Generate")]
    public GameObject creatableMesh;
    public GameObject creatableSpawnMesh;

    [HideInInspector]
    public int seed;

    private NavMeshSurface meshSurface;
    private GameObject _mesh;
    private bool isGenerated = false;
    public bool isSpawnRoom = false;

    private void Start()
    {
        if (isSpawnRoom) _mesh = Instantiate(creatableSpawnMesh, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                if (!isGenerated && !isSpawnRoom)
                {
                    _mesh = Instantiate(creatableMesh, transform);

                    var generator = _mesh.transform.Find("WallGenerator").GetComponent<LVWallGenerator>();
                    generator.clusterID = seed;
                    generator.StartGenerate();

                    //bakeryamaker
                    meshSurface = GameObject.Find("BakerYaMaker").GetComponent<NavMeshSurface>();
                    meshSurface.BuildNavMesh();

                    isGenerated = true;
                }

                if (!_mesh.activeSelf) _mesh.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<NetworkingPlayerController>() && obj.GetComponent<NetworkingPlayerController>().view.IsMine)
        {
            _mesh.SetActive(!_mesh.activeSelf);
        }    
    }
}