using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    public GameManager gameManager;
    public Transform playerTransform;
    public bool sendDir = false;
    [Header("OSC Properties")]
    public string sourceName;
    public float sourceRadius;
    public int maxListSize;

    private int sampleRate = 2;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.registerOSC(sourceName, maxListSize);

        InvokeRepeating(nameof(SendData), 0f, sampleRate * Time.deltaTime);
    }

    void SendData()
    {
        // Send distance --> volume
        Vector3 playerPos = playerTransform.position;
        Vector3 sourcePos = GetComponent<Transform>().position;
        float dist = Vector3.Distance(playerPos, sourcePos);
        float vol = Mathf.Clamp01(1f / (1f + dist * dist / (sourceRadius * sourceRadius))); // Inverse-Square Law
        gameManager.sendOSCVol(sourceName, vol);

        // Send direction --> panning
        if (!sendDir) return;
        Vector3 playerToSource = sourcePos - playerPos;
        Vector3 playerDir = playerTransform.InverseTransformDirection(playerToSource);
        gameManager.sendOSCDir(sourceName, new Vector2(playerDir.x, playerDir.z));  // y is UP !!!
    }
}
