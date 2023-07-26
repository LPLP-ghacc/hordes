using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstaMenu : MonoBehaviour
{
    public KeyCode OpenKey;

    public GameObject @object;

    public void Update()
    {
        if (Input.GetKeyDown(OpenKey)) @object.SetActive(!@object.activeSelf);
    }

    public void SetToggleActive(GameObject localObject)
    {
        localObject.SetActive(!localObject.activeSelf);
    }

    public void OnExitButton()
    {
        @object.SetActive(!@object.activeSelf);
    }
}
