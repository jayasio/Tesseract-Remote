using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerState
{
    public Vector3 position;
    public Quaternion rotation;
    // public bool tracking = false;

    public string SerializeState()
    {
        return JsonUtility.ToJson(this);
    }

    public TrackerState DeserializeState(string data)
    {
        return JsonUtility.FromJson<TrackerState>(data);
    }
}
