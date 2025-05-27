using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CDR;

public class WorldBuilder : MonoBehaviour
{
    [Header("Weights")]
    public float minScore = 1.0f;
    public WeightPoint[] weightPoints;

    [Header("Dimensions")]

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

    private void Start()
    {
        //initializing the weightPointsLastFrame 
        weightPointsLastFrame = new WeightPoint[weightPoints.Length];
        for (int i = 0; i < 3; i++)
        {
            weightPointsLastFrame[i] = new WeightPoint(0, new Vector3());
        }

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

    public GameObject CreateBox(Vector3 pos)
    {
        Vector3[] vertices = {
            new Vector3 (0,0,0),
            new Vector3 (1,0,0),
            new Vector3 (1,1,0),
            new Vector3 (0,1,0),
            new Vector3 (0,1,1),
            new Vector3 (1,1,1),
            new Vector3 (1,0,1),
            new Vector3 (0,0,1),
        };
        int[] triangles = {
            0, 2, 1, //face front
	        0, 3, 2,
            2, 3, 4, //face top
	        2, 4, 5,
            1, 2, 5, //face right
	        1, 5, 6,
            0, 7, 4, //face left
	        0, 4, 3,
            5, 4, 7, //face back
	        5, 7, 6,
            0, 6, 7, //face bottom
	        0, 1, 6
        };

        GameObject newBlock = new();
        Mesh newMesh = newBlock.AddComponent<MeshFilter>().mesh;
        Renderer rend = newBlock.AddComponent<MeshRenderer>();
        rend.material = mat;
        newMesh.Clear();
        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.Optimize();
        newMesh.RecalculateNormals();
        newBlock.transform.position = pos;
        return newBlock;
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
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int d = 0; d < height; d++)
                {
                    //this is the first target for optimization,
                    //  new objects shouldn't be created for each voxel,
                    //  instead, a new set of vertices constructing a cube
                    //  should be added to an existing mesh
                    GameObject newCube = CreateBox(new Vector3(w, h, d)); //GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //newCube.transform.position = new Vector3(w, h, d);
                    //if (mat != null)
                    //{
                    //    newCube.GetComponent<MeshRenderer>().material = mat;
                    //}
                    voxelSet.Add(newCube);
                }
                if (visualizeBuild)
                {
                    SetVisibilityByWeight();
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        worldComplete = true;
        SetVisibilityByWeight();
        yield break;

    }

}
