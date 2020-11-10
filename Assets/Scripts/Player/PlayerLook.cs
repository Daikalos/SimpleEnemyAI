using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField, Range(0.0f, 20.0f)]
    private float m_MouseSensitivity = 1.7f;

    private Camera m_Camera;
    private Vector3 m_CameraRotation;

    void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity;

        m_CameraRotation.x -= mouseY;
        m_CameraRotation.x = Mathf.Clamp(m_CameraRotation.x, -90f, 90f);

        m_Camera.transform.localRotation = Quaternion.Euler(m_CameraRotation);
        m_Camera.transform.parent.Rotate(Vector3.up * mouseX);
    }
}
