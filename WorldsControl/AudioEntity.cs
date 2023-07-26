using UnityEngine;

public class AudioEntity : MonoBehaviour
{
    private GameObject pursuitTarget = null;

    private protected void Start()
    {
        do
        {
            pursuitTarget = GameObject.FindGameObjectWithTag("Player");

        } while (pursuitTarget != null);
    }

    private protected void Update()
    {
        if(pursuitTarget != null)
        {
            transform.position = pursuitTarget.transform.position;
        }
    }
}
