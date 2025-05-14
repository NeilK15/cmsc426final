using UnityEngine;

[System.Serializable]
public class DetectionData
{
    public WristData left;
    public WristData right;

    public static DetectionData FromJson(string json)
    {
        return JsonUtility.FromJson<DetectionData>(json);
    }
}

[System.Serializable]
public class WristData
{
    public bool detected;
    public float x;
    public float y;
}