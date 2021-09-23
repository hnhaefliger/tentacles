using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using static Chunk;

public class TerrainChunk
{
    int chunkSize;
    float threshold;
    float multiplier;
    float maxViewDistance;
    float maxStoreDistance;

    GameObject meshObject;
    Vector3 position;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    bool generated;

    public TerrainChunk(int x, int y, int z, int tmpChunkSize, float tmpThreshold, float tmpMultiplier, float tmpMaxViewDistance, float tmpMaxStoreDistance, Material material)
    {
        position = new Vector3(x, y, z);
        chunkSize = tmpChunkSize;
        threshold = tmpThreshold;
        multiplier = tmpMultiplier;
        maxViewDistance = tmpMaxViewDistance;
        maxStoreDistance = tmpMaxStoreDistance;
        mesh = new Mesh();
        mesh.Clear();

        meshObject = new GameObject("Chunk" + position.ToString());
        meshObject.AddComponent<MeshFilter>();
        meshObject.AddComponent<MeshRenderer>();
        meshObject.AddComponent<MeshCollider>();

        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = material;

        generated = false;

        ThreadStart threadStart = delegate {
            GeneratorThread();
        };

        new Thread(threadStart).Start();
    }

    public void GeneratorThread()
    {
        (vertices, triangles) = Chunk.GenerateChunk((int)(position.x*chunkSize), (int)(position.y*chunkSize), (int)(position.z*chunkSize), chunkSize, threshold, multiplier);
        generated = true;
    }

    public bool UpdateMesh()
    {
        if (generated)
        {
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.Optimize();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshObject.GetComponent<MeshCollider>().sharedMesh = mesh;

            SetVisible(true);

            generated = false;

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Update(Vector3 playerPosition)
    {
        float dist = (float)Vector3.Distance(position, playerPosition);

        if (dist < maxViewDistance) {
            SetVisible(true);
            return false;
        }
        else
        {
            if (dist < maxStoreDistance)
            {
                SetVisible(false);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }
}
