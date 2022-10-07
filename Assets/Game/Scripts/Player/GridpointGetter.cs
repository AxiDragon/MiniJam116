using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridpointGetter : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    GridGenerator generator;

    void Start()
    {
        generator = FindObjectOfType<GridGenerator>();
    }

    public GridPoint GetClosestPoint(bool occ)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return generator.GetClosestSnapPoint(hit.point, occ);
        }

        return null;
    }
}
