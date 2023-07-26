using JetBrains.Annotations;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingVoiceControlling : MonoBehaviour
{
    private Recorder recorder;

    [Space]
    public KeyCode enableVoice = KeyCode.V;

    [Space]
    public bool isHoldToSpeak = true;

    public bool recordWhenJoined = false;

    public bool isPlayerSpeak = false;

    public bool debugEcho = false;

    void Start()
    {
        recorder = this.gameObject.transform.Find("Recorder").GetComponent<Recorder>();

        recorder.RecordWhenJoined = recordWhenJoined;
    }

    void Update()
    {
        recorder.DebugEchoMode = debugEcho;

        if (isHoldToSpeak)
        {
            if (Input.GetKey(enableVoice))
                recorder.TransmitEnabled = true;

            if(Input.GetKeyUp(enableVoice))
                recorder.TransmitEnabled = false;

            isPlayerSpeak = recorder.TransmitEnabled;

        }
    }
}
