using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    [HideInInspector] public MeshRenderer mr;

    private void Start()
    {
        mr = transform.Find("Outline").GetComponent<MeshRenderer>();
        mr.enabled = false;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                mr.enabled = true;
            }
        }
        else
        {
            mr.enabled = false;
        }
    }
}
