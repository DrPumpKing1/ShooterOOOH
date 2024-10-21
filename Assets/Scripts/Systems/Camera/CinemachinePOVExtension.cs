using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("Parameters - Cursor")]
    [SerializeField] private bool cursorVisible;
    [SerializeField] private CursorLockMode cursorLockMode;

    [Header("Parameters - Rotation")]
    [SerializeField] private float lookAroundSpeedHorizontal;
    [SerializeField] private float lookAroundSpeedVertical;
    [SerializeField] private float maxPolarAngle;
    [SerializeField] private float minPolarAngle;

    [Header("Properties")]
    [SerializeField] private Vector2 mouseDelta;
    [SerializeField] private Vector3 startingRotation;

    [Header("Extra Components")]
    [SerializeField] private Transform orientation;

    protected override void Awake()
    {
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLockMode;

        base.Awake();
    }

    public void OnLookAround(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Aim || !vcam.Follow) return;

        if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;

        startingRotation.x += mouseDelta.x * Time.deltaTime * lookAroundSpeedHorizontal;
        startingRotation.y -= mouseDelta.y * Time.deltaTime * lookAroundSpeedVertical;
        startingRotation.y = Mathf.Clamp(startingRotation.y, minPolarAngle, maxPolarAngle);
        state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);

        orientation.rotation = Quaternion.Euler(0f, startingRotation.x, 0f);
    }
}
