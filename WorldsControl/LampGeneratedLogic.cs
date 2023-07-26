using UnityEngine;

public class LampGeneratedLogic : MonoBehaviour
{
    public Material materialOnWall;
    private Rigidbody rb;
    private GameObject lightObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        lightObject = transform.GetComponentInChildren<Light>().gameObject;
    }

    private void Unlight()
    {
        gameObject.GetComponent<Renderer>().material = materialOnWall;

        Destroy(lightObject);
        Destroy(rb);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            Unlight();
        }
    }
}