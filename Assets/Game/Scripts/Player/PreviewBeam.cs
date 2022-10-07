using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBeam : MonoBehaviour
{
    GridpointGetter getter;

    private void Awake()
    {
        getter = FindObjectOfType<GridpointGetter>();
    }

    private void Start()
    {
        transform.parent = null;
        transform.position = Vector3.one * 999f;
    }

    void Update()
    {
        GridPoint closest = getter.GetClosestPoint(false);

        if (closest != null)
            transform.position = closest.transform.position;
    }
}
