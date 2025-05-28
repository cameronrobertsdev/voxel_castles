using System;
using UnityEngine;

public class ArrayTests : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //createAndEditIntArray();
        CreateAndEditVector3Array();
    }

    public void CreateAndEditVector3Array()
    {
        Vector3[] vector3Array = new Vector3[12000];
        Debug.Log($"vector3Array initialized: {string.Join(" ", vector3Array)}");
        vector3Array[0] = new Vector3(1, 1, 1);
        vector3Array[1] = new Vector3(2, 2, 2);
        Debug.Log($"vector3Array after setting [0] and [1]: {string.Join(" ", vector3Array)}");

        Vector3[] v3Array2 = new Vector3[12000];
        Debug.Log($"v3Array2 initialized: {string.Join(" ", v3Array2)}");

        foreach (Vector3 i in v3Array2)
        {
            i.Set(3, 3, 3);
        }
        Debug.Log($"v3Array2 after assigning from a foreach: {string.Join(" ", v3Array2)}");

        for (int i = 0; i < v3Array2.Length; i++)
        {
            v3Array2[i].Set(i, i, i);
        }
        Debug.Log($"v3Array2 after assigning from a for loop: {string.Join(" ", v3Array2)}");

        for (int i = 0; i < vector3Array.Length; i++)
        {
            vector3Array[i] = v3Array2[i];
        }
        Debug.Log($"vector3Array after assigning corresponding values from v3Array2 in a for loop : {string.Join(" ", vector3Array)}");

        Vector3[] shorterArray = new Vector3[3];

        for (int i = 0; i < shorterArray.Length ; i++) 
        {
            vector3Array[vector3Array.Length - 1 - i] = shorterArray[i];
        }
        Debug.Log($"vector3Array after assigning corresponding values from short array in a for loop : {string.Join(" ", vector3Array)}");

    }

    public void createAndEditIntArray()
    {
        int[] intArray = new int[10];
        print($"intArray initialized: {string.Join(" ", intArray)}");
        intArray[0] = 23;
        intArray[1] = 27;
        print($"intArray after setting [0] and [1]: {string.Join(" ", intArray)}");

        int[] intArray2 = new int[10];
        print($"intArray2 initialized: {string.Join(" ", intArray2)}");

        foreach (int i in intArray2)
        {
            intArray2[i] = 3;
        }
        print($"intArray2 after assigning from a foreach: {string.Join(" ", intArray2)}");

        for (int i = 0; i < intArray2.Length; i++)
        {
            intArray2[i] = i;
        }
        print($"intArray2 after assigning from a for loop: {string.Join(" ", intArray2)}");

        for (int i = 0; i < intArray.Length; i++)
        {
            intArray[i] = intArray2[i];
        }
        print($"intArray after assigning corresponding values from intArray2 in a for loop : {string.Join(" ", intArray)}");
    }
}
