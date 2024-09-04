using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//Fast Poisson Disk Sampling
//https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

namespace KUsystem.Utils
{
    public class PoissonDiskSampling : MonoBehaviour
    {
        //[SerializeField] private Vector3 world3d;

        public static List<Vector3> GeneratePoints(float radius, Vector3 samplingRegionSize, int numSamplesBeforeRejection = 30)
        {
            float cellSize = radius / Mathf.Sqrt(3);
            int[,,] grid = new int[Mathf.CeilToInt(samplingRegionSize.x / cellSize), Mathf.CeilToInt(samplingRegionSize.y / cellSize), Mathf.CeilToInt(samplingRegionSize.z / cellSize)];
            //points we will generate
            List<Vector3> points = new List<Vector3>();
            //points from world which generated
            List<Vector3> spawnPoints = new List<Vector3>();

            spawnPoints.Add(samplingRegionSize / 2);
            while (spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector3 spawnCenter = spawnPoints[spawnIndex];
                bool candidateAccepted = false;
                for (int i = 0; i < numSamplesBeforeRejection; i++)
                {
                    float angle = Random.value * Mathf.PI * 2;
                    float forz = Random.Range(-1f, 1f);
                    //radom direction
                    Vector3 dir = new Vector3(Mathf.Sqrt(1 - forz * forz) * Mathf.Cos(angle), Mathf.Sqrt(1 - forz * forz) * Mathf.Sin(angle), forz);
                    Vector3 candidate = spawnCenter + dir * Random.Range(radius, 2 * radius);
                    if (IsValid(candidate, samplingRegionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize), (int)(candidate.z / cellSize)] = points.Count;
                        candidateAccepted = true;
                        break;
                    }
                }
                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }

            }
            return points;
        }


        static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
        {
            if (candidate.x >= 0 && candidate.x <= sampleRegionSize.x && candidate.y >= 0 && candidate.y <= sampleRegionSize.y && candidate.z >= 0 && candidate.z <= sampleRegionSize.z)
            {
                int cellX = (int)(candidate.x / cellSize);
                int cellY = (int)(candidate.y / cellSize);
                int cellZ = (int)(candidate.z / cellSize);

                int startX = Mathf.Max(0, cellX - 2), startY = Mathf.Max(0, cellY - 2), startZ = Mathf.Max(0, cellZ - 2);
                int endX = Mathf.Min(grid.GetLength(0) - 1, cellX + 2), endY = Mathf.Min(grid.GetLength(1) - 1, cellY + 2), endZ = Mathf.Min(grid.GetLength(2) - 1, cellZ + 2);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int z = startZ; z <= endZ; z++)
                        {
                            int pointIndex = grid[x, y, z] - 1;
                            if (pointIndex != -1)
                            {
                                float dst = (candidate - points[pointIndex]).sqrMagnitude;
                                if (dst < radius * radius)
                                    return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
