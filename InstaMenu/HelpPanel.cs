using Assets._NETWORK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public TextMeshProUGUI objname, key;

    public GameObject visiblePart;

    private void Start()
    {
        key.text = HorDesKeys.interactKey.ToString();
    }

    public void Show(string objectName)
    {
        visiblePart.SetActive(true);

        GetComponent<Image>().enabled = true;

        objname.text = objectName;
    }

    public void Hide()
    {
        visiblePart.SetActive(false);

        GetComponent<Image>().enabled = false;

        objname.text = "";
    }
}
