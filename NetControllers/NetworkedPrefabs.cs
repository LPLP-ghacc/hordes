using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkedPrefabs
{
    public GameObject prefab;

    public string path;

    public NetworkedPrefabs(GameObject obj, string _path) 
    {
        prefab = obj;
        path = ReturnPrefabPathModified(_path);
    }

    private string ReturnPrefabPathModified(string path)
    {
        int extenLength = Path.GetExtension(path).Length;

        int startIndex = path.ToLower().IndexOf("Resources");

        if (startIndex == -1) 
        {
            return string.Empty;
        }else
        {
            return path.Substring(startIndex, path.Length - (startIndex + extenLength));
        }
    }
}

