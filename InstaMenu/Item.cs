using Photon.Pun;
using UnityEngine;

namespace Assets._NETWORK
{
    public static class GameExtention
    {
        /// <summary>
        /// Destroy this object if the object below -100
        /// </summary>
        public static void OnFall(this GameObject obj, float height = -100)
        {
            if(obj.transform.position.y < height)
                PhotonNetwork.Destroy(obj.gameObject);
        }
    }
}
