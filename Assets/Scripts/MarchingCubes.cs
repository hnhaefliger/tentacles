using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections;
using static MarchingCubesConstants;

public static class MarchingCubes
{
    public static (Vector3[], int[]) March(bool[] cubes, int x, int y, int z, int chunkSize) {
        Vector3[] vertices = new Vector3[cubes.Length*12];
        int nVertices = 0;
        int[] triangles = new int[cubes.Length*12];
        int nTriangles = 0;

        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                for (int k = 0; k < chunkSize; k++)
                {
                    int cubeindex = 0;
                    if (cubes[i*(chunkSize+1)*(chunkSize+1) + j*(chunkSize+1) + k]) cubeindex += 1;
                    if (cubes[(i+1)*(chunkSize+1)*(chunkSize+1) + j*(chunkSize+1) + k]) cubeindex += 2;
                    if (cubes[(i+1)*(chunkSize+1)*(chunkSize+1) + j*(chunkSize+1) + k+1]) cubeindex += 4;
                    if (cubes[i*(chunkSize+1)*(chunkSize+1) + j*(chunkSize+1) + k+1]) cubeindex += 8;
                    if (cubes[i*(chunkSize+1)*(chunkSize+1) + (j+1)*(chunkSize+1) + k]) cubeindex += 16;
                    if (cubes[(i+1)*(chunkSize+1)*(chunkSize+1) + (j+1)*(chunkSize+1) + k]) cubeindex += 32;
                    if (cubes[(i+1)*(chunkSize+1)*(chunkSize+1) + (j+1)*(chunkSize+1) + k+1]) cubeindex += 64;
                    if (cubes[i*(chunkSize+1)*(chunkSize+1) + (j+1)*(chunkSize+1) + k+1]) cubeindex += 128;

                    for (int l = 0; l < 12; l++)
                    {
                        int a = MarchingCubesConstants.table[cubeindex, l];

                        if (a < 0)
                        {
                            break;
                        }

                        Vector3 tmpVertex = new Vector3(i + MarchingCubesConstants.edges[a, 0] + x, j + MarchingCubesConstants.edges[a, 1] + y, k + MarchingCubesConstants.edges[a, 2] + z);

                        int b = Array.IndexOf(vertices, tmpVertex);

                        if (b > -1) {
                            triangles[nTriangles] = b;
                            nTriangles += 1;
                        }
                        else
                        {
                            vertices[nVertices] = tmpVertex;
                            triangles[nTriangles] = nVertices;
                            nTriangles += 1;
                            nVertices += 1;
                        }
                    }
                }
            }
        }

        Vector3[] cleanVertices = new Vector3[nVertices];
        int[] cleanTriangles = new int[nTriangles];

        for (int i = 0; i < nVertices; i++) {
            cleanVertices[i] = vertices[i];
        }

        for (int i = 0; i < nTriangles; i++) {
            cleanTriangles[i] = triangles[i];
        }

        return (cleanVertices, cleanTriangles);
    }
  }
