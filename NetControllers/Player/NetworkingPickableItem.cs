using Assets._NETWORK;
using UnityEngine;

public class NetworkingPickableItem : MonoBehaviour
{
    public string ID = "";

    public string Count = "1";

    public void PickUp()
    {
        if (!NetworkingPlayerController.GetLocalPlayer().IsMine)
            return;

        if (ID == "" && int.Parse(Count) == 0)
            return;

        GameObject.FindGameObjectWithTag("Player").GetComponent<NetInventory>().AddItem(new NetItem(ID, Count));

        Destroy(gameObject);
    }

    protected void Update()
    {
        gameObject.OnFall();
    }
}