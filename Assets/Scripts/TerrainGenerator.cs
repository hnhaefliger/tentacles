using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Chunk;
using static TerrainChunk;

public class TerrainGenerator : MonoBehaviour
{
    public int chunkSize = 20;
    public float threshold = 0.3f;
    public float multiplier = 0.1f;
    public float maxViewDistance = 1f;
    public float maxStoreDistance = 4f;
    public Transform viewer;

    Vector3 viewerPosition;
    TerrainChunk chunk;

    Dictionary<Vector3, TerrainChunk> chunks = new Dictionary<Vector3, TerrainChunk>();
    List<Vector3> ungenerated = new List<Vector3>();

    void Start()
    {
        viewerPosition = new Vector3(
            (int)Mathf.RoundToInt(viewer.position.x / chunkSize - 0.5f),
            (int)Mathf.RoundToInt(viewer.position.y / chunkSize - 0.5f),
            (int)Mathf.RoundToInt(viewer.position.z / chunkSize - 0.5f)
        );

        CreateNewChunks();
        UpdateChunks();
    }

    void Update()
    {
        GenerateChunks();

        Vector3 newViewerPosition = new Vector3(
            (int)Mathf.RoundToInt(viewer.position.x / chunkSize - 0.5f),
            (int)Mathf.RoundToInt(viewer.position.y / chunkSize - 0.5f),
            (int)Mathf.RoundToInt(viewer.position.z / chunkSize - 0.5f)
        );

        if (newViewerPosition != viewerPosition)
        {
            viewerPosition = newViewerPosition;
            CreateNewChunks();
            UpdateChunks();
        }
    }

    void CreateNewChunks()
    {
        for (int i = (int)Mathf.FloorToInt(viewerPosition.x - maxViewDistance); i < (int)Mathf.FloorToInt(viewerPosition.x + maxViewDistance + 1); i++)
        {
            for (int j = (int)Mathf.FloorToInt(viewerPosition.y - maxViewDistance); j < (int)Mathf.FloorToInt(viewerPosition.y + maxViewDistance + 1); j++)
            {
                for (int k = (int)Mathf.FloorToInt(viewerPosition.z - maxViewDistance); k < (int)Mathf.FloorToInt(viewerPosition.z + maxViewDistance + 1); k++)
                {
                    Vector3 position = new Vector3(i, j, k);

                    if (!chunks.ContainsKey(position))
                    {
                        chunks.Add(position, new TerrainChunk(i, j, k, chunkSize, threshold, multiplier, maxViewDistance, maxStoreDistance));
                        ungenerated.Add(position);
                    }
                }
            }
        }
    }

    void UpdateChunks()
    {
        Vector3[] marked = new Vector3[chunks.Count];
        int i = 0;

        foreach (Vector3 key in chunks.Keys)
        {
            if (chunks[key].Update(viewerPosition))
            {
                marked[i] = key;
                i += 1;
            }
        }

        for (int j = 0; j < i; j++)
        {
            chunks.Remove(marked[j]);
        }
    }

    void GenerateChunks()
    {
        Vector3 marked = new Vector3(0, 0, 0);

        foreach (Vector3 key in ungenerated)
        {
            if (chunks.ContainsKey(key))
            {
                if (chunks[key].UpdateMesh())
                {
                    marked = key;
                    break;
                }
            }
            else
            {
                marked = key;
                break;
            }
        }

        ungenerated.Remove(marked);
    }
}
