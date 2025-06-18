using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, List<float>> srcToVol;
    private Dictionary<string, List<Vector2>> srcToDir;
    private Dictionary<string, int> capacity;

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler.Instance.Init();

        srcToVol = new Dictionary<string, List<float>>();
        srcToDir = new Dictionary<string, List<Vector2>>();
        capacity = new Dictionary<string, int>();
    }

    public void registerOSC(string srcName, int maxSize)
    {
        if (!srcToVol.ContainsKey(srcName))
            srcToVol.Add(srcName, new List<float>());
        if (!srcToDir.ContainsKey(srcName))
            srcToDir.Add(srcName, new List<Vector2>());
        if (!capacity.ContainsKey(srcName))
            capacity.Add(srcName, maxSize);
    }

    public void sendOSCVol(string srcName, float volume)
    {
        if (!srcToVol.ContainsKey(srcName) || !capacity.ContainsKey(srcName))
            return;

        List<float> vol = srcToVol[srcName];
        while (vol.Count >= capacity[srcName])
            vol.RemoveAt(0);
        vol.Add(volume);

        float maxV = float.MinValue;
        foreach (float v in vol)
            maxV = Mathf.Max(maxV, v);

        OSCHandler.Instance.SendMessageToClient("Max", srcName + "_vol", maxV);
    }

    public void sendOSCDir(string srcName, Vector2 direction)
    {
        if (!srcToDir.ContainsKey(srcName) || !capacity.ContainsKey(srcName))
            return;

        List<Vector2> dir = srcToDir[srcName];
        while (dir.Count >= capacity[srcName])
            dir.RemoveAt(0);
        dir.Add(direction);

        Vector2 minD = Vector2.zero;
        float minMag2 = float.MaxValue;
        foreach (Vector2 d in dir)
        {
            float dMag2 = d.sqrMagnitude;
            if (dMag2 < minMag2)
            {
                minD = d;
                minMag2 = dMag2;
            }
        }

        List<float> minDList = new List<float>();
        minDList.Add(minD.x);
        minDList.Add(minD.y);
        OSCHandler.Instance.SendMessageToClient("Max", srcName + "_dir", minDList);
    }
}
