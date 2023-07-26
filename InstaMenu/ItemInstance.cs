using Assets._NETWORK;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstance : MonoBehaviour
{
    public string ID = "";
    public string count;
    public Image image;
    public TextMeshProUGUI textbox;
    public TextMeshProUGUI countTextbox;

    private Action action;

    private protected void Start()
    {
        var attributes = NetIDConvert.GetAttributesByID(ID);

        textbox.text = attributes.Item1;

        image.sprite = attributes.Item2;

        action = attributes.Item3;

        countTextbox.text = count;

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        action();

        if(int.Parse(count) - 1 == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            count = $"{int.Parse(count) - 1}";
        }
    }
}
