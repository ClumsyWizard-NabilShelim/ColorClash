using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Touch,
    MouseAndKeyboard
}

public class InputManager : Persistant<InputManager>
{
    [SerializeField] private InputType inputType;
    public static List<Vector2> PointerWorldPositions { get; private set; } = new List<Vector2>();
    public static bool IsPointerMoving { get; private set; }
    private Action onSignificantPointerMove;

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        switch (inputType)
        {
            case InputType.Touch:
                Touch();
                break;
            case InputType.MouseAndKeyboard:
                MouseAndKeyboard();
                break;
            default:
                break;
        }
    }

    private void Touch()
    {
        if(Input.touchCount > 0)
        {
            IsPointerMoving = true;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (i > PlayerDataManager.PlayerStats.Blades - 1)
                    break;

                if (Input.touches[i].deltaPosition.magnitude > 0)
                    onSignificantPointerMove?.Invoke();

                if (PointerWorldPositions.Count < i + 1)
                    PointerWorldPositions.Add(CameraHelper.Camera.ScreenToWorldPoint(Input.touches[i].position));
                else
                    PointerWorldPositions[i] = CameraHelper.Camera.ScreenToWorldPoint(Input.touches[i].position);
            }
        }
        else
        {
            IsPointerMoving = false;
        }
    }
    private void MouseAndKeyboard()
    {
        if(Input.GetMouseButton(0))
        {
            IsPointerMoving = true;

            if (PointerWorldPositions.Count == 0)
                PointerWorldPositions.Add(CameraHelper.Camera.ScreenToWorldPoint(Input.mousePosition));
            else
                PointerWorldPositions[0] = CameraHelper.Camera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            IsPointerMoving = false;
        }
    }

    public static void OnSignificantPointerMove(Action callback)
    {
        Instance.onSignificantPointerMove += callback;
    }

    protected override void CleanUp()
    {
        onSignificantPointerMove = null;
    }
}