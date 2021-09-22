using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Chunk;
using static TerrainChunk;

public class TerrainGenerator : MonoBehaviour
{
    public int chunkSize = 10;
    public float threshold = 0.3f;
    public float multiplier = 0.1f;
    public float maxViewDistance = 2f;
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

    Vector3[] CreateNeighborChunks(Vector3 position)
    {
        if (Vector3.Distance(viewerPosition, position) < maxStoreDistance)
        {
            if (!chunks.ContainsKey(position))
            {
                chunks.Add(position, new TerrainChunk((int)position.x, (int)position.y, (int)position.z, chunkSize, threshold, multiplier, maxViewDistance, maxStoreDistance));
                ungenerated.Add(position);
            }

            return new Vector3[] {
                position + new Vector3(1, 0, 0),
                position + new Vector3(-1, 0, 0),
                position + new Vector3(0, 1, 0),
                position + new Vector3(0, -1, 0),
                position + new Vector3(0, 0, 1),
                position + new Vector3(0, 0, -1)
            };
        }
        else
        {
            return new Vector3[] {position};
        }
    }

    void CreateNewChunks()
    {
        List<Vector3> toCreate = new List<Vector3>{viewerPosition};
        List<Vector3> created = new List<Vector3>();

        while (toCreate.Count > 0)
        {
            foreach (Vector3 position in CreateNeighborChunks(toCreate[0]))
            {
                if (!toCreate.Contains(position) && !created.Contains(position))
                {
                    toCreate.Add(position);
                }
            }

            created.Add(toCreate[0]);
            toCreate.RemoveAt(0);
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
