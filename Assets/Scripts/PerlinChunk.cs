using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perlin;

public static class PerlinChunk
{
    public static bool[] GenerateChunk(int x, int y, int z, int chunkSize, float threshold, float multiplier)
    {
        bool[] cubes = new bool[(int)Mathf.Pow(chunkSize + 1, 3)];

        for (int i = 0; i < chunkSize + 1; i++)
        {
            for (int j = 0; j < chunkSize + 1; j++)
            {
                for (int k = 0; k < chunkSize + 1; k++)
                {
                    cubes[i*(chunkSize+1)*(chunkSize+1) + j*(chunkSize+1) + k] = Perlin.Noise((x+i+0.123f)*multiplier, (y+j+0.123f)*multiplier, (z+k+0.123f)*multiplier) > threshold;
                }
            }
        }

        return cubes;
    }
}
