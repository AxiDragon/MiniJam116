using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5f;

    void Update()
    {
        transform.eulerAngles += Vector3.up * Time.deltaTime * rotationSpeed; 
    }
}
