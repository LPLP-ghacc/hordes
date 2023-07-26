using Assets._NETWORK;
using UnityEngine;

public class LightOptimizer : MonoBehaviour
{
    public float maxDistance = 45f;
    public float normalLightIntensity = 8f;
    public float lightFadeDistance = 10f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 playerPosition = player.position;

        Light[] lights = FindObjectsOfType<Light>();

        foreach (Light light in lights)
        {
            float distance = Vector3.Distance(light.transform.position, playerPosition);

            float delta = normalLightIntensity - Mathf.Clamp(distance / (maxDistance + lightFadeDistance), 0, 1) * normalLightIntensity;

            light.enabled = delta > 0;

            light.intensity = normalLightIntensity * delta;
        }
    }
}
