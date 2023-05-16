using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public static GameInput Instance { get; private set; }

    public event EventHandler OnDrawingStarted;
    public event EventHandler OnDrawingFinished;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.DrawPath.performed += DrawPath_performed;
        _playerInputActions.Player.DrawPath.canceled += DrawPath_canceled;
    }

    private void DrawPath_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDrawingStarted?.Invoke(this, EventArgs.Empty);
    }

    private void DrawPath_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDrawingFinished?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _playerInputActions.Dispose();
    }
}