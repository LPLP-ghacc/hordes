using UnityEngine;

public class NetworkingItemController : MonoBehaviour
{
    public GameObject itemPrefab;

    private void OnEnable()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        var items = GameObject.FindGameObjectWithTag("Player").GetComponent<NetInventory>().items;

        items.ForEach(item => {
            var itemComp = Instantiate(itemPrefab, transform).GetComponent<ItemInstance>();

            itemComp.ID = item.ID;
            itemComp.count = item.Count;
        });
    }
}
