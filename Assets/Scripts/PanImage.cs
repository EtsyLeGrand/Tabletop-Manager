using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanImage : MonoBehaviour
{
    [SerializeField] private UIManager uiManager; // Pour l'action Lock
    [SerializeField] private float panSpeed = 0.005f;
    private bool isPanning = false;
    private bool isLocked = false;
    private bool isModeAppropriate = false;

    private int xRotationModifier = 1;
    private int yRotationModifier = 1;
    private bool isImageOnTheSide = false;

    private Vector3 lastPanPosition;

    private void Start()
    {
        uiManager.OnLockButtonClicked += () => { isLocked = !isLocked; };
        uiManager.OnMouseModeChanged += (UIManager.MouseMode newMode) => {
            if (newMode == UIManager.MouseMode.Pan) isModeAppropriate = true;
            else isModeAppropriate = false;
        };
        uiManager.OnRotationChanged += (int newRotation) =>
        {
            xRotationModifier = (newRotation == 0 || newRotation == 270) ? 1 : -1;
            yRotationModifier = (newRotation == 0 || newRotation == 90) ? 1 : -1;
            isImageOnTheSide = (newRotation == 90 || newRotation == 270);
        };
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !isLocked && isModeAppropriate)
        {
            isPanning = true;
            lastPanPosition = Input.mousePosition;
        }
    }

    void OnMouseUp()
    {
        isPanning = false;
    }

    void OnMouseDrag()
    {
        if (isPanning)
        {
            Vector3 panDelta = Input.mousePosition - lastPanPosition;

            /* 0: x*1 y*1
             * 90: y*1 x*-1
             * 180: x*-1 y*-1
             * -90: y*-1 x*1 */

            Vector3 adjustedPanDelta;
            if (!isImageOnTheSide)
                adjustedPanDelta = new Vector3(panDelta.x * xRotationModifier, panDelta.y * yRotationModifier, panDelta.z);
            else
                adjustedPanDelta = new Vector3(panDelta.y * yRotationModifier, panDelta.x * xRotationModifier, panDelta.z);
                
            transform.Translate(adjustedPanDelta * panSpeed);
            lastPanPosition = Input.mousePosition;
        }
    }
}
