using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandsSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(gameObject.transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;

            //NetworkingItem item = dropped.GetComponent<NetworkingItem>();
            //item.parentAfterDrag = transform;
            //
            //item.UseItem();
        }

    }
}
