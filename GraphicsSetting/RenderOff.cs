using UnityEngine;

public class RenderOff : MonoBehaviour
{
    private Camera cam;

    private int TargetFrameRate = 5;
    private Rect standartRect;
    private int standartFrameRate;
    private int standartvSync;

    private void Start()
    {
        cam = this.gameObject.GetComponent<Camera>();
        standartRect = cam.pixelRect;
        standartFrameRate = Application.targetFrameRate;
        standartvSync = QualitySettings.vSyncCount;

        if (!cam)
            Destroy(this.gameObject.GetComponent<RenderOff>());


    }

    private void OnApplicationFocus(bool focus)
    {
        #if !UNITY_EDITOR
        if (focus)
        {
            cam.pixelRect = standartRect;

            QualitySettings.vSyncCount = standartvSync;
            Application.targetFrameRate = standartFrameRate;
        }
        else
        {
            cam.pixelRect = new Rect(cam.transform.position.x, cam.transform.position.y, 1f, 1f);

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = TargetFrameRate;
        }
        #endif
    }
}
