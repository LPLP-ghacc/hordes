using System.Collections.Generic;
using UnityEngine;

public class NetInventory : MonoBehaviour
{
    [HideInInspector]
    public List<NetItem> items = new List<NetItem>();

    public void AddItem(NetItem item)
    {
        items.Add(item);

        GameObject.FindGameObjectWithTag("INVENTORY")?.GetComponent<NetworkingItemController>().RefreshInventory();
    }
}
