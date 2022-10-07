using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridFishMover : MonoBehaviour
{
    [SerializeField] UnityEvent actEvent;
    [SerializeField] float yOffset = 2f;
    GridpointGetter getter;
    FishMovement fish;

    void Start()
    {
        fish = FindObjectOfType<FishMovement>();
        getter = FindObjectOfType<GridpointGetter>();
    }

    public void TryMoveFish(InputAction.CallbackContext callback)
    {
        if (!callback.performed)
            return;

        MoveFish(getter.GetClosestPoint(true).transform.position);
        actEvent.Invoke();
    }

    public void MoveFish(Vector3 position)
    {
        Vector3 destination = position + Vector3.up * yOffset;
        //fish.StartMove(destination);
    }
}
