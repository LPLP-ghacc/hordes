using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerBySave : MonoBehaviour
{
    private void Update()
    {
        var resol = GameObject.Find("[ADMINPANEL]").GetComponent<LocalhostPanel>().screenResolution;

        gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(resol.x, resol.y);
    }
}
