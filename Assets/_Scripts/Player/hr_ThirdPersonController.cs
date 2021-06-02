using UnityEngine;
using UnityEngine.Animations.Rigging;

public class hr_ThirdPersonController : MonoBehaviour
{
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isJumping = false;
    public bool isGrounded = false;

    [Header("Physics Settings")]
    [SerializeField] private float groundDrag = 5.0f;
    [SerializeField] private float airDrag = 1.0f;
    [SerializeField] private float movementMultiplier = 70.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.0f;
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 2.0f;
    [SerializeField] private float lowerDist = 0.2f;
    [SerializeField] private float upperDist = 0.4f;
    [SerializeField] private float lowerDistAngle = 0.3f;
    [SerializeField] private float upperDistAngle = 0.5f;

    [Header("Camera Settings")]
    [SerializeField] private Transform lookAt;
    [SerializeField] private Cinemachine.AxisState xAxis;
    [SerializeField] private Cinemachine.AxisState yAxis;
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private float sensitivityX = 1.0f;
    [SerializeField] private float sensitivityY = 1.0f;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float cameraZoomDefault = 40.0f;
    [SerializeField] private float cameraZoom = 20.0f;
    [SerializeField] private float cameraZoomSpeed = 10.0f;

    [Header("Aim Settings")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private float aimTimeMultiplier = 10.0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float walkingSpeed = 1.0f;
    [SerializeField] private float runningSpeed = 2.0f;
    [SerializeField] private float jumpForce = 2.0f;

    [Header("Stealth Settings")]
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private float shootSoundIntensity = 12.0f;
    [SerializeField] private float baseStealthProfile = 1.5f;
    [SerializeField] private float sprintSoundIntensity = 3f;
    [SerializeField] private float walkSoundIntensity = 2f;

    // References
    private Rigidbody rigiBody;
    private Animator animator;
    private Camera mainCamera;
    private hr_InputManager inputManager;
    private hr_RaycastWeapon weapon;
    private SphereCollider sphereCollider;

    // Helper variables
    private Vector3 moveDirection = Vector3.zero;
    private bool aimState = false;
    private bool firingState = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Get a reference to this object's hr_InputManager.
        inputManager = this.GetComponent<hr_InputManager>();

        // Get a reference to this object's rigid body.
        rigiBody = this.GetComponent<Rigidbody>();

        // Get a reference to this object's animator.
        animator = this.GetComponent<Animator>();

        // Get a reference to the weapon's object.
        weapon = GetComponentInChildren<hr_RaycastWeapon>();

        // Get a reference to the stealth sphere collider object.
        sphereCollider = this.transform.Find("StealthCollider").GetComponent<SphereCollider>();

        // Get a reference to main camera's transform.
        mainCamera = Camera.main;

        // Update the player's rigid body drag.
        rigiBody.drag = groundDrag;

        // Set the upper step height.
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        HandleCameraRotation();
        HandlePlayerRotation();
        HandlePlayerAiming();
        HandleWeaponFiring();

        AlertNearbyZombies();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        CheckForGround();
        ControlDrag();
        HandleMovement();
        HandleJump();
        StepClimb();
    }

    /// <summary>
    /// Changes the rigid body's drag according on where the player is.
    /// </summary>
    private void ControlDrag()
    {
        if (isGrounded)
        {
            rigiBody.drag = groundDrag;
        }
        else
        {
            rigiBody.drag = airDrag;
            rigiBody.AddForce(new Vector3(0, -150000f, 0) * Time.deltaTime);
        }
    }

    /// <summary>
    /// This function handles the player movement and should be called on FixedUpdate, since it
    /// uses Unity's physics system and rigid body to add movement. It also sets the animator
    /// floats to control the player's movement animations.
    /// </summary>
    private void HandleMovement()
    {
        moveDirection = transform.forward * inputManager.movementInput.y + transform.right * inputManager.movementInput.x;

        if (moveDirection != Vector3.zero)
        {
            isWalking = true;
            moveSpeed = walkingSpeed;
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }

        if (isWalking && inputManager.sprintInput)
        {
            isRunning = true;
            moveSpeed = runningSpeed;
        }
        else
        {
            isRunning = false;
        }

        rigiBody.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);

        Vector3 rbVelocity = transform.InverseTransformDirection(rigiBody.velocity);

        animator.SetFloat("SpeedZ", rbVelocity.z, 0.1f, Time.deltaTime);
        animator.SetFloat("SpeedX", rbVelocity.x, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// This functions handles the player's jump. Whenever the player presses the jump key it adds
    /// a sudden impulse to the rigid boyd, simulating a jump.
    /// </summary>
    private void HandleJump()
    {
        if (isGrounded && inputManager.jumpInput)
        {
            rigiBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// This function checks if the player is touching the ground. It sends a raycast straight down
    /// looking for anything in that matches the given ground layer.
    /// </summary>
    private void CheckForGround()
    {
        // Debug.DrawRay(transform.position + new Vector3(0, groundCheckDistance / 2, 0), Vector3.down * groundCheckDistance, Color.red);
        isGrounded = Physics.Raycast(transform.position + new Vector3(0, groundCheckDistance / 2, 0), Vector3.down, groundCheckDistance, groundLayer);
    }

    /// <summary>
    /// This function matches the player's rotation to the main camera's rotation.
    /// </summary>
    private void HandlePlayerRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookAt.position - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This function updates the cameraLookAt object to mirror the mouse movements, this way
    /// the main camera can follow it's orientation.
    /// </summary>
    private void HandleCameraRotation()
    {
        xAxis.Value += inputManager.mouseLook.x * sensitivityX;
        yAxis.Value += inputManager.mouseLook.y * sensitivityY * (-1.0f);
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }

    /// <summary>
    /// This function updates the rig weights when the player aims.
    /// </summary>
    private void HandlePlayerAiming()
    {
        if (inputManager.aimInput)
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1.0f, Time.deltaTime * aimTimeMultiplier);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraZoom, Time.deltaTime * cameraZoomSpeed);

            if (!aimState)
            {
                aimState = true;
            }
        }
        else
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0.0f, Time.deltaTime * aimTimeMultiplier);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraZoomDefault, Time.deltaTime * cameraZoomSpeed);

            if (aimState)
            {
                aimState = false;
            }
        }
    }

    /// <summary>
    /// This function calls the weapon script's to start firing.
    /// </summary>
    private void HandleWeaponFiring()
    {
        if (aimState)
        {
            if (inputManager.fireInput)
            {
                if (!firingState)
                {
                    firingState = true;
                    weapon.StartFiring();
                }
            }
            else
            {
                if (firingState)
                {
                    firingState = false;
                    weapon.StopFiring();
                }
            }
        }
    }

    /// <summary>
    /// Creates a sphere around the player, with the radius being the shootSoundIntensity.
    /// All the zombies inside this sphere will become aware of the player.
    /// </summary>
    private void AlertNearbyZombies()
    {
        sphereCollider.radius = baseStealthProfile * GetPlayerStealthProfile();

        if (firingState)
        {
            Collider[] zombies = Physics.OverlapSphere(transform.position, shootSoundIntensity, zombieLayer);

            foreach (var zombie in zombies)
            {
                zombie.GetComponent<hr_ZombieController>().OnAware();
            }
        }
    }

    /// <summary>
    /// Returns the stealth profile of the player.
    /// </summary>
    private float GetPlayerStealthProfile()
    {
        if (isWalking)
        {
            if (isRunning)
            {
                return sprintSoundIntensity;
            }
            else
            {
                return walkSoundIntensity;
            }
        }
        else
        {
            return 1;
        }
    }

    private void StepClimb()
    {
        Debug.DrawRay(stepRayLower.transform.position, transform.forward * lowerDist, Color.green);
        RaycastHit lowerHit;
        if (Physics.Raycast(stepRayLower.transform.position, transform.forward, out lowerHit, lowerDist))
        {
            Debug.DrawRay(stepRayUpper.transform.position, transform.forward * upperDist, Color.green);
            RaycastHit upperHit;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.forward, out upperHit, upperDist))
            {
                rigiBody.position -= new Vector3(0.0f, -stepSmooth * Time.deltaTime, 0.0f);
            }
        }

        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1) * lowerDistAngle, Color.green);
        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, lowerDistAngle))
        {
            Debug.DrawRay(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1) * upperDistAngle, Color.green);
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, upperDistAngle))
            {
                rigiBody.position -= new Vector3(0.0f, -stepSmooth * Time.deltaTime, 0.0f);
            }
        }

        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1) * lowerDistAngle, Color.green);
        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, lowerDistAngle))
        {
            Debug.DrawRay(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1) * upperDistAngle, Color.green);
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, upperDistAngle))
            {
                rigiBody.position -= new Vector3(0.0f, -stepSmooth * Time.deltaTime, 0.0f);
            }
        }
    }
}
