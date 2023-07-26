using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._NETWORK
{
    public static class LocalPlayer 
    {
        /// <summary>
        /// Mandatory naming of the local player object as LOCALPLAYER
        /// </summary>
        /// <returns></returns>
        public static PhotonView GetLocalView()
        {
            return PhotonView.Get(GameObject.Find(PlayersFieds.localPLayerName));
        }

        /// <summary>
        /// Mandatory naming of the localhost adminpanel object as [ADMINPANEL]
        /// </summary>
        /// <returns></returns>
        public static LocalhostPanel GetLocalhostPanel()
        {
            if (GameObject.Find("[ADMINPANEL]"))
            {
                return GameObject.Find("[ADMINPANEL]").GetComponent<LocalhostPanel>();
            }else
            {
                return null;
            }
            
        }

        public static GameObject GetLocalPLayerObject()
        {
            return GameObject.Find(PlayersFieds.localPLayerName).gameObject;
        }

        public static NetworkingPlayerController GetLocalController()
        {
            return GetLocalPLayerObject().GetComponent<NetworkingPlayerController>();
        }
    }
}
