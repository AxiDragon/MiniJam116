using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockTest : MonoBehaviour
{
    public void Place(Vector3 position)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = position;
    }
}
