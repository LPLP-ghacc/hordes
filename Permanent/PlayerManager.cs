using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject localPlayerGo;

        public PhotonView playerView;

        private void Start()
        {
            this.gameObject.name = "_PLAYERFIELDS";
        }
    }
}
