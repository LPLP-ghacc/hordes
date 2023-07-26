using Photon.Pun;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private Camera localPlayerCamera;

    private Quaternion playerRotation;

    public KeyCode sprayMarker = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(sprayMarker))
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkingPlayerController>() && player.GetComponent<NetworkingPlayerController>().isSelf)
                {
                    localPlayerCamera = player.GetComponent<NetworkingPlayerController>().camPrefab.GetComponent<Camera>();

                    playerRotation = player.transform.rotation;
                }
            }

            Ray ray = localPlayerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                if (hit.collider != null)
                {
                    var decal = Resources.Load("MARKERDECAL");

                    decal = decal as GameObject;

                    PhotonNetwork.Instantiate(decal.name, hit.point, playerRotation);
                }
            }
        }
    }
}
