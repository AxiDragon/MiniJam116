using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetMovement : MonoBehaviour
{
    Vector3 moveDirection = Vector3.zero;
    Transform movingCamera;
    ScrollZoom offsetSource;
    [SerializeField] float speed = 5f;

    void Start()
    {
        movingCamera = Camera.main.transform;
        offsetSource = movingCamera.transform.parent.GetComponentInChildren<ScrollZoom>();
    }

    void Update()
    {
        Vector3 rotatedInput = Quaternion.AngleAxis(movingCamera.eulerAngles.y, Vector3.up) * moveDirection;
        transform.Translate(rotatedInput * speed * Time.deltaTime * offsetSource.offset);
    }

    public void SetMove(InputAction.CallbackContext callback)
    {
        Vector2 input = callback.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0f, input.y);
    }
}
