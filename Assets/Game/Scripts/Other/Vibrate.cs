using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        Vector3 rand = GetRandomVector(2f);
        transform.localPosition = startPos + rand;
    }

    private Vector3 GetRandomVector(float magnitude)
    {
        float x = UnityEngine.Random.Range(-magnitude, magnitude);
        float y = UnityEngine.Random.Range(-magnitude, magnitude);
        float z = UnityEngine.Random.Range(-magnitude, magnitude);
        return new Vector3(x, y, z);
    }
}
