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

    //public void MakeWorld()
    //{
    //    for (int w = 0; w < width; w++)
    //    {

    //        for (int h = 0; h < height; h++)
    //        {
    //            for (int d = 0; d < height; d++)
    //            {
    //                Vector3 currentPos = new Vector3(w, h, d);

    //                if (true) //(CompareAgainstWeightPoints(currentPos))
    //                {
    //                    GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                    newCube.transform.position = new Vector3(w, h, d);
    //                    if (mat != null)
    //                    {
    //                        newCube.GetComponent<MeshRenderer>().material = mat;
    //                    }
    //                    voxelSet.Add(newCube);
    //                }
    //            }
    //        }
    //    }
    //    worldComplete = true;
    //}

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
                    GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    newCube.transform.position = new Vector3(w, h, d);
                    if (mat != null)
                    {
                        newCube.GetComponent<MeshRenderer>().material = mat;
                    }
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
