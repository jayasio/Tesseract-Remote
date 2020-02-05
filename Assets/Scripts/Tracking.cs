using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tracking : MonoBehaviour
{
    TrackerState trackerState;
    TCPTestClient tcp;

    public bool tracking {get; set;}
    public TMP_InputField IpInputField, PortInputField;


    void Start()
    {
        tcp = new TCPTestClient();
        trackerState = new TrackerState();
    }

    public void OnConnect() => tcp.OnConnect(IpInputField.text, int.Parse(PortInputField.text));

    void Update()
    {
        if (tracking && tcp.connected)
        {
            trackerState.position = Camera.main.transform.InverseTransformDirection(transform.position - Camera.main.transform.position);
            trackerState.rotation = transform.rotation;
            tcp.SendTracking(trackerState.SerializeState());
        }
    }

    void OnGUI()
    {
        if (tracking)
        {
            GUI.Label(new Rect(100, 100, 200, 200), "x " + trackerState.position.x + "   y " + trackerState.position.y + "   z " + trackerState.position.z);
            GUI.Label(new Rect(100, 200, 200, 200), "x " + trackerState.rotation.x + "   y " + trackerState.rotation.y + "   z " + trackerState.rotation.z + "   w " + trackerState.rotation.w);
        }
        GUI.Label(new Rect(100, 300, 200, 200), "Current status " + tracking);
    }
}
