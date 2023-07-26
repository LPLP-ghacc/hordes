using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InstaButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetAction(UnityAction action)
    {
        var button = GetComponent<Button>();

        button.onClick.AddListener(action);
    }

    public void SetText(string message)
    {
        text.text = message;
    }
}
