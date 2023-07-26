using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalRes : MonoBehaviour
{
    public List<GameObject> resourceItems = new List<GameObject>();
    public DefaultPool pool;

    private static string tagName = "localres";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        this.tag = tagName;

        this.name = "LRES";

        var list = Resources.LoadAll<GameObject>("").ToList();

        list.ForEach(resourceItems.Add);

        pool = PhotonNetwork.PrefabPool as DefaultPool;

        if (pool != null && resourceItems != null)
        {
            foreach (GameObject prefab in resourceItems)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }

        Debug.Log($"Resources ({resourceItems.Count}) have been added in {this.name} gameObject. LocalRes comp.");
    }

    public static LocalRes ReturnLocalRes()
    {
        return GameObject.FindGameObjectWithTag(tagName).GetComponent<LocalRes>();
    }
}
