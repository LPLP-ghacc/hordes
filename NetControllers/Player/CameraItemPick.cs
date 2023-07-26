using Assets._NETWORK;
using UnityEngine;

public class CameraItemPick : MonoBehaviour
{
    private Camera localcam;
    private HelpPanel helpPanel;

    public bool enable = true;

    [SerializeField]
    private float raycastDistance = 1.7f;

    [HideInInspector]
    public float delay = 0f;

    private void Start()
    {
        localcam = GetComponent<Camera>();

        helpPanel = GameObject.FindGameObjectWithTag("HelpPanel").GetComponent<HelpPanel>();
    }

    private void Update()
    {
        if (!NetworkingPlayerController.GetLocalPlayer().IsMine)
            return;

        if(delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            Ray ray = localcam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                NetworkingPickableItem item = hit.collider.GetComponent<NetworkingPickableItem>();

                if (item != null)
                {
                    if (Input.GetKeyDown(HorDesKeys.interactKey))
                    {
                        item.PickUp();
                        delay += 1f;
                    }
                    else
                    {
                        helpPanel.Show(NetIDConvert.GetAttributesByID(item.ID).Item1);
                    }
                }
                else
                {
                    helpPanel.Hide();
                }
            }
        }
    }
}
