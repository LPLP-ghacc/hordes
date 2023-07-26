using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWobble : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;
    [HideInInspector]
    public NetworkingPlayerController controller;

    [SerializeField] private readonly float intensity = 0.5f;
    [SerializeField] private readonly float amplitude = .03f;

    private Vector3 nextSwayVector;
    private Vector3 nextSwayPosition;
    private Vector3 startLocalPosition;

    protected void Start()
    {
        cam = GetComponent<Camera>();

        nextSwayVector = Vector3.up * amplitude;
        nextSwayPosition = transform.localPosition + nextSwayVector;
        startLocalPosition = transform.localPosition;
    }

    protected void Update()
    {
        if (!controller || !cam)
            return;

        if(controller.isWalking)
        {
            cam.transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextSwayPosition, intensity * Time.deltaTime);

            if (Vector3.SqrMagnitude(cam.transform.localPosition - nextSwayPosition) < 0.01f)
            {
                nextSwayVector = -nextSwayVector;

                nextSwayPosition = startLocalPosition + nextSwayVector;
            }
        }else
        {
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, startLocalPosition, intensity * Time.deltaTime);
        }
    }
}
