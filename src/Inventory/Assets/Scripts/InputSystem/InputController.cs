using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private InputActions inputActions;

    [HideInInspector] public Vector2 directionV2, mouseLookV2Direction;
    [HideInInspector] public bool isMove, pressM1, pressTap;

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Player_Controller.MovementV2.started += OnMove;
        inputActions.Player_Controller.MovementV2.performed += OnMove;
        inputActions.Player_Controller.MovementV2.canceled += OnMove;

        inputActions.UI_Controller.InventoryOpen.started += OnOpenInventory;
        inputActions.UI_Controller.InventoryOpen.canceled += OnOpenInventory;

        inputActions.Camera_Controller.MouseX.performed += OnMouseX;
        inputActions.Camera_Controller.MouseY.performed += OnMouseY;
    }

    private void Update()
    {
        pressM1 = OnMouse();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        directionV2 = context.ReadValue<Vector2>();

        isMove = directionV2.x != 0 || directionV2.y != 0;
    }

    private void OnMouseX(InputAction.CallbackContext context) => mouseLookV2Direction.x = context.ReadValue<float>();

    private void OnMouseY(InputAction.CallbackContext context) => mouseLookV2Direction.y = context.ReadValue<float>();

    private void OnOpenInventory(InputAction.CallbackContext context) => pressTap = context.ReadValueAsButton();

    private bool OnMouse() => Mouse.current.leftButton.wasReleasedThisFrame;

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Player_Controller.Enable();
            inputActions.Camera_Controller.Enable();
            inputActions.UI_Controller.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player_Controller.Disable();
            inputActions.Camera_Controller.Disable();
            inputActions.UI_Controller.Disable();
        }
    }
}
