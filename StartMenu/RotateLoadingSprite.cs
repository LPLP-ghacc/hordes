using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoadingSprite : MonoBehaviour
{
    private float value = 0;

    void Update()
    {
        value += Time.deltaTime;

        this.gameObject.transform.rotation = Quaternion.Euler(0, 0 , value);
    }
}
