using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class AfterloadCameraController : MonoBehaviour
    {
        private Camera currentCam;

        protected void Start()
        {
            if (SceneManager.GetActiveScene().name == "LEVEL0")
            {
                currentCam = GetComponent<NetworkingPlayerController>().camPrefab;
                currentCam.enabled = false;

                StartCoroutine(ZeroLevel(3));
                StartCoroutine(Pipapipa());
            }
        }

        private IEnumerator ZeroLevel(float time)
        {
            yield return new WaitForSeconds(time);

            var meshes = GameObject.FindGameObjectsWithTag("SpawnableMesh");
            var meshesCount = meshes.Length;
            int completedMeshes = 0;

            foreach(var mesh in meshes)
            {
                if(mesh.GetComponent<WallGenerate>().isGenerationComplete)
                    completedMeshes++;
            }

            if (meshesCount == completedMeshes)
                currentCam.enabled = true;
            else
                StartCoroutine(ZeroLevel(1));

            Destroy(this.gameObject.GetComponent<AfterloadCameraController>());
        }


        private IEnumerator Pipapipa()
        {
            yield return new WaitForSeconds(5);

            currentCam.enabled = true;

            Destroy(this.gameObject.GetComponent<AfterloadCameraController>());
        }
    }
}
