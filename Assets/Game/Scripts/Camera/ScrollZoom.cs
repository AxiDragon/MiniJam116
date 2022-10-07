using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ScrollZoom : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    [HideInInspector] public float offset;
    [SerializeField] float sensitivity = 5f;
    [SerializeField] float adjustSpeed = 5f;
    [SerializeField] float minZoom = 1f;
    [SerializeField] float maxZoom = 35f;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        offset = vcam.m_Lens.OrthographicSize;
    }

    private void FixedUpdate()
    {
        float difference = offset - vcam.m_Lens.OrthographicSize;
        vcam.m_Lens.OrthographicSize += difference * adjustSpeed / 100f;
    }

    public void UpdateOffset(InputAction.CallbackContext callback)
    {
        float value = -callback.ReadValue<float>();
        offset += value * sensitivity / 100f;
        offset = Mathf.Clamp(offset, minZoom, maxZoom);
    }
}
