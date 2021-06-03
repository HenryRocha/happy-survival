using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hr_InputManager : MonoBehaviour
{
    [Header("Inputs")]
    public Vector2 movementInput = Vector2.zero;
    public Vector2 mouseLook = Vector2.zero;
    public bool sprintInput;
    public bool jumpInput;
    public bool aimInput;
    public bool fireInput;
    public bool pauseInput;

    private PlayerControls playerControls;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.Mouse.MouseLook.performed += i => mouseLook = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Jump.canceled += i => jumpInput = false;

            playerControls.PlayerActions.Aim.performed += i => aimInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimInput = false;

            playerControls.PlayerActions.Fire.performed += i => fireInput = true;
            playerControls.PlayerActions.Fire.canceled += i => fireInput = false;

            playerControls.PlayerActions.Pause.performed += i => pauseInput = true;
            playerControls.PlayerActions.Pause.canceled += i => pauseInput = false;
        }

        playerControls.Enable();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
