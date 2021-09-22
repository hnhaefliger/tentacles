using UnityEngine;
using static PerlinChunk;
using static MarchingCubes;

public static class Chunk
{
    public static (Vector3[], int[]) GenerateChunk(int x, int y, int z, int chunkSize, float threshold, float multiplier)
    {
        return MarchingCubes.March(PerlinChunk.GenerateChunk(x, y, z, chunkSize, threshold, multiplier), x, y, z, chunkSize);
    }
}
