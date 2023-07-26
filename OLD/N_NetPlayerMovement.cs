using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class N_NetPlayerMovement : MonoBehaviour
{
    #region Variables
    private GameObject self;
    #region Player's variables
    [Header("PUBLIC PLAYERS FLOATS")]
    public float playerWalkVelocity;
    public float playerRunVelocity;
    public float playerJumpForce;
    public float playerCooldownTime;
    public float playerSendDamageCooldownTime;
    public float maxVelocityChange = 10f;
    #endregion

    #region Keys
    [Header("KEY BINDS")]
    public KeyCode runKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;
    public KeyCode sendDamageKey;
    public KeyCode altSendDamageKey;
    public KeyCode defendKey;
    #endregion

    #region Player components
    [HideInInspector]
    public Animator playerAnimator;
    [HideInInspector]
    public Rigidbody playerRigidbody;
    [HideInInspector]
    public Camera playerCamera;
    [HideInInspector]
    public Collider playerCapsuleCollider;
    #endregion

    #region Player states
    [Header("PLAYER STATES")]
    public bool isWalk = false;
    public bool isRun = false;
    public bool isJump = false;
    public bool isCrouch = false;
    public bool isSendDamage = false;
    public bool isAltSendDamage = false;
    public bool isResieveDamage = false;
    public bool isFall = false;
    public bool isGrounded = false;
    public bool isDie = false;
    public bool isInteract = false;
    #endregion

    #region Camera
    [HideInInspector]
    private float yaw = 0.0f;
    [HideInInspector]
    public float maxLookAngle = 50f;
    [HideInInspector]
    private float pitch = 0.0f;
    [Header("CAMERA SETTINGS")]
    public float fov = 60f;
    public float mouseSensitivity = 2f;
    #endregion
    #endregion

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        GetAllPlayerComponents();
    }

    private void Update()
    {
        CameraLook();

        Movement();
    }

    private void Movement()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

       
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(runKey))
        {
            AddForceToPlayer(playerWalkVelocity, targetVelocity);
            isWalk = true;    
        }
        else isWalk = false;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(runKey))
        {
            AddForceToPlayer(playerRunVelocity, targetVelocity);
            isRun = true;     
        }
        else isRun = false;

    }

    private void AddForceToPlayer(float thisVelocity, Vector3 targetVelocity)
    {
        targetVelocity = transform.TransformDirection(targetVelocity) * thisVelocity;

        Vector3 velocity = playerRigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0; 

        playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public bool CheckGround(float targetDistance)
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = targetDistance;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            Log("player grounded");
            return true;
        }
        else
        {
            Log("player is not grounded");
            return false;
        }
    }

    private void CameraLook()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        // Move self
        self.transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    private void GetAllPlayerComponents()
    {
        if (this.gameObject.GetComponent<Collider>())
            playerCapsuleCollider = this.gameObject.GetComponent<Collider>();
        else Log("Player Collider cant be found", 1);

        if (this.gameObject.GetComponent<Rigidbody>())
            playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        else Log("Player Rigidbody cant be found", 1);

        if (transform.Find("Mesh").GetComponent<Animator>())
            playerAnimator = this.gameObject.transform.Find("Mesh").GetComponent<Animator>();
        else Log("Player Mesh(Animator) cant be found or rename this GameObject like Mesh", 1);

        if (this.gameObject.transform.Find("Eye_PlayerCamera").GetComponent<Camera>())
            playerCamera = this.gameObject.transform.Find("Eye_PlayerCamera").GetComponent<Camera>();
        else Log("Player Camera cant be found or rename this GameObject like Eye_PlayerCamera", 1);

        self = this.gameObject;
    }

    private void Log(object message, int type = 0)
    {
        if(type == 0) Debug.Log(message);
        else Debug.LogError(message);
    }
}
