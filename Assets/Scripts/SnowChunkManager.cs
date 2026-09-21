using System.Collections.Generic;
using UnityEngine;

public class SnowChunkManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform snowChunkParent;
    private Camera[] chunkCameras;
    private Vector3[] chunkCenters; // cache once, avoid transform.position lookups
    [SerializeField]
    private float activateDistance = 40f;
    [SerializeField]
    private float checkInterval = 0.3f;

    private float nextCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Camera> camerasList = new List<Camera>();
        List<Vector3> centersList = new List<Vector3>();

        foreach (Transform child in snowChunkParent.transform)
        {
            if (child.childCount == 0) continue;

            Transform grandChild = child.GetChild(0);
            Camera cam = grandChild.GetComponent<Camera>();

            if (cam == null) continue;

            camerasList.Add(cam);
            centersList.Add(grandChild.position);
        }

        chunkCameras = camerasList.ToArray();
        chunkCenters = centersList.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        nextCheck += Time.deltaTime;
        if (nextCheck < checkInterval) return;

        nextCheck = 0f;
        float sqrDist = activateDistance * activateDistance;
        Vector3 p = player.position;

        for (int i = 0; i < chunkCameras.Length; i++)
        {
            bool shouldBeActive = (chunkCenters[i] - p).sqrMagnitude <= sqrDist;
            if (chunkCameras[i].enabled != shouldBeActive)
            {
                chunkCameras[i].enabled = shouldBeActive;
            }
        }
    }
}
