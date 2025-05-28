using CDR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    [Header("Weights")]
    public float minScore = 1.0f;
    public WeightPoint[] weightPoints;

    [Header("Dimensions")]

    public int gap = 2;

    [Range(1, 30)]
    public int width = 10;
    [Range(1, 30)]
    public int height = 10;
    [Range(1, 30)]
    public int depth = 10;

    [Header("Visualization")]
    public bool visualizeBuild = false;
    public Material mat;

    //newly created voxels are added to this list to enable access later
    private List<GameObject> voxelSet = new List<GameObject>();
    //an array that is intended to be used during testing to only update the visibility of voxels when
    //  the weight values are changed
    private WeightPoint[] weightPointsLastFrame;
    //flag to be set after world build finishes
    private bool worldComplete = false;

    private Mesh worldMesh;

    private void Start()
    {
        //initializing the weightPointsLastFrame 
        weightPointsLastFrame = new WeightPoint[weightPoints.Length];
        //for (int i = 0; i < 3; i++)
        //{
        //    weightPointsLastFrame[i] = new WeightPoint(0, new Vector3());
        //}
        worldMesh = GetComponent<MeshFilter>().mesh;
        worldMesh.Clear();
        worldMesh.vertices = new Vector3[height * width * depth * 8];
        worldMesh.triangles = new int[height * width * depth * 36];
        StartCoroutine(nameof(MakeWorld));
       
    }

    private void Update()
    {
        if (worldComplete)
        {
            //TODO: fix weight point lists being evaluated as not equal when evaluated

            //if (!weightPoints.SequenceEqual(weightPointsLastFrame))
            //{
            //    print("sequences not equal");
            //    SetVisibilityByWeight();
            //    for (int i = 0; i <= weightPoints.Length - 1; i++)
            //    {
            //        weightPointsLastFrame[i].weight = weightPoints[i].weight;
            //        weightPointsLastFrame[i].position = weightPoints[i].position;
            //    }
            //}
        }
    }

    public void AddBoxToExistingMesh(Vector3 pos, int voxelIndex) 
    {
        int vertIndex = voxelIndex * 8;
        int tIndex = voxelIndex * 36;

        Vector3[] newVertices = {
            new Vector3 (0,0,0) + pos,
            new Vector3 (1,0,0) + pos,
            new Vector3 (1,1,0) + pos,
            new Vector3 (0,1,0) + pos,
            new Vector3 (0,1,1) + pos,
            new Vector3 (1,1,1) + pos,
            new Vector3 (1,0,1) + pos,
            new Vector3 (0,0,1) + pos
        };

        int[] newTriangles = {
            voxelIndex, voxelIndex + 2,voxelIndex +  1, //face front
	        voxelIndex, voxelIndex + 3, voxelIndex + 2,
            voxelIndex + 2, voxelIndex + 3, voxelIndex + 4, //face top
	        voxelIndex + 2, voxelIndex + 4, voxelIndex + 5,
            voxelIndex + 1, voxelIndex + 2, voxelIndex + 5, //face right
	        voxelIndex + 1, voxelIndex + 5, voxelIndex + 6,
            voxelIndex, voxelIndex + 7, voxelIndex + 4, //face left
	        voxelIndex, voxelIndex + 4, voxelIndex + 3,
            voxelIndex + 5, voxelIndex + 4, voxelIndex + 7, //face back
	        voxelIndex + 5, voxelIndex + 7, voxelIndex + 6,
            voxelIndex, voxelIndex + 6, voxelIndex + 7, //face bottom
	        voxelIndex, voxelIndex + 1, voxelIndex + 6
        };
        //print($"new vertices: {newVertices.Length}");

        for (int i = 0; i < newVertices.Length; i++)
        {
            //print($"{vertIndex + i} vertex before {worldMesh.vertices[i + vertIndex]}");
            //print($"new vertex {vertIndex} : {newVertices[i]}");
            worldMesh.vertices[i + vertIndex] = newVertices[i];
            //print($"{vertIndex + i} vertex after {worldMesh.vertices[i + vertIndex]}");
        }

        for (int i = 0; i < newTriangles.Length; i++)
        {
            print($"tris before {worldMesh.triangles[i + tIndex]}");
            print($"new triangle {newTriangles[i]}");
            worldMesh.triangles[i + tIndex] = newTriangles[i];
            print($"tris after {worldMesh.triangles[i + tIndex]}");
        }
    }

    public void SetVisibilityByWeight()
    {
        foreach (GameObject currentVoxel in voxelSet)
        {
            currentVoxel.SetActive(CompareAgainstWeightPoints(currentVoxel.transform.position));
        }
    }

    public bool CompareAgainstWeightPoints(Vector3 pos)
    {
        float totalScore = 0;
        if (weightPoints.Length > 0)
        {
            foreach (WeightPoint wp in weightPoints)
            {
                totalScore += wp.weight * (1 / Mathf.Pow(Vector3.Distance(pos, wp.position), 2));
            }
            //print($"New Block: {totalScore}");
        }
        return totalScore >= minScore;
    }

    IEnumerator MakeWorld()
    {
        int voxelIndex = 0;
        for (int w = 0; w < width; w += gap)
        {
            for (int h = 0; h < height; h += gap)
            {
                for (int d = 0; d < height; d += gap)
                {
                   
                    AddBoxToExistingMesh(new Vector3(w,h,d),voxelIndex);
                    voxelIndex++;
                }
                if (visualizeBuild)
                {
                    //SetVisibilityByWeight();
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        worldMesh.RecalculateBounds();
        //worldMesh.Optimize();
        worldMesh.RecalculateNormals();
        worldComplete = true;
        print("world complete");
        //SetVisibilityByWeight();
        yield break;

    }

}
