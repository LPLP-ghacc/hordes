using Assets;
using Assets._NETWORK;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class NetworkingPlayerController : MonoBehaviour
{
    [Space(10f)]
    [Header("Controller")]
    public bool isSelf = false;
    [HideInInspector]
    public PhotonView view;
    public GameObject UI;
    public List<GameObject> localHide;
    public GameObject rightHand;
    private GameObject mesh;

    [Header("Camera")]
    public Camera camPrefab;
    public bool isHeadPivot = false;
    [Space]
    private readonly float fov = 60f;
    private readonly bool cameraCanMove = true;
    private readonly float mouseSensitivity = 2f;
    private readonly float maxLookAngle = 50f;

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    public bool enableZoom = true;
    public bool holdToZoom = false;

    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;
    private bool _isZoomed = false;

    [Header("Movement")]
    public float walkSpeed = 3f;
    private readonly float maxVelocityChange = 20f;
    public float sprintSpeed = 7f;
    private readonly float sprintFOV = 80f;
    private readonly bool enableJump = true;
    private readonly float jumpPower = 5f;
    private readonly bool enableCrouch = true;
    private readonly bool holdToCrouch = true;
    private readonly float speedReduction = .5f;

    public bool isWalking = false;
    public bool isSprinting = false;
    public bool isGrounded = false;
    public bool isCrouched = false;
    public bool isGamePause = false;
    public bool isPlayerDead = false;
    public bool isInteract = false;

    [HideInInspector]
    public NetworkingItemController itemController;
    private Rigidbody _rigi;

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    protected void Start()
    {
        if (view.IsMine)
        {
            PhotonView.DontDestroyOnLoad(view);

            OnStartController();

            gameObject.name = "LOCALPLAYER";
        }
        else
        {
            gameObject.name = "NETWORKPLAYER";
            return;
        }

        gameObject.AddComponent<AfterloadCameraController>();
    }

    private void OnStartController()
    {
        isSelf = true;

        //Get mesh 
        mesh = transform.Find("[PIVOT]/[MESH]").gameObject;

        //hide objs
        LocalHide(false);

        var camSpawnPoint = transform.Find("[PIVOT]/[CAMERAPIVOT]");

        //init cam
        if (camPrefab)
        {
            camPrefab = Instantiate(camPrefab, camSpawnPoint.position, Quaternion.identity, camSpawnPoint);
        }
        else
        {
            camPrefab = camSpawnPoint.gameObject.AddComponent<Camera>();
        }

        camPrefab.fieldOfView = fov;

        //init UI
        UI = Instantiate(UI, camPrefab.transform.position + new Vector3(0, 0, 2.2f), Quaternion.identity, camPrefab.transform);
        UI.name = "UI";
        UI.SetActive(true);

        _rigi = gameObject.GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        if (!view.IsMine)
            return;

        PauseGame();
        OnUpdate();
        CheckGround();
        Unslip();
        CameraHeadPivot();
        gameObject.OnFall();

        isInteract = Input.GetKey(HorDesKeys.interactKey);
    }

    protected void FixedUpdate()
    {
        if (!view.IsMine)
            return;

        Move();
    }

    private void Unslip()
    {
        camPrefab.transform.localPosition = Vector3.zero;
        mesh.transform.localPosition = Vector3.zero;
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (isGrounded)
        {
            if (Input.GetKey(HorDesKeys.sprint))
            {
                isSprinting = true;
                camPrefab.fieldOfView = Mathf.Lerp(camPrefab.fieldOfView, sprintFOV, 2 * Time.deltaTime);

                _rigi.AddForce(VelocityChange(targetVelocity, sprintSpeed), ForceMode.VelocityChange);
            }
            else
            {
                isSprinting = false;

                _rigi.AddForce(VelocityChange(targetVelocity, walkSpeed), ForceMode.VelocityChange);
            }
        }

        if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
        {
            isSprinting = Input.GetKey(HorDesKeys.sprint);
            isWalking = !isSprinting;
        }
        else
        {
            isWalking = false;
            isSprinting = false;
        }
    }

    private Vector3 VelocityChange(Vector3 targetVelocity, float speed)
    {
        targetVelocity = transform.TransformDirection(targetVelocity) * speed;

        Vector3 velocity = _rigi.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    private void OnUpdate()
    {
        if (cameraCanMove)
        {
            _yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            _pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            _pitch = Mathf.Clamp(_pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, _yaw, 0);
            camPrefab.transform.localEulerAngles = new Vector3(_pitch, 0, 0);
        }

        if (enableZoom)
        {
            if (Input.GetKeyDown(HorDesKeys.zoomKey) && !holdToZoom && !isSprinting)
                _isZoomed = !_isZoomed;

            if (holdToZoom && !isSprinting)
            {
                if (Input.GetKeyDown(HorDesKeys.zoomKey))
                    _isZoomed = true;
                else if (Input.GetKeyUp(HorDesKeys.zoomKey))
                    _isZoomed = false;
            }

            if (_isZoomed)
                camPrefab.fieldOfView = Mathf.Lerp(camPrefab.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            else if (!_isZoomed && !isSprinting)
                camPrefab.fieldOfView = Mathf.Lerp(camPrefab.fieldOfView, fov, zoomStepTime * Time.deltaTime);
        }

        if (enableJump && Input.GetKeyDown(HorDesKeys.jump) && isGrounded)
            Jump();

        if (enableCrouch)
        {
            if (Input.GetKeyDown(HorDesKeys.crouch) && !holdToCrouch)
                SpeedReduction();

            if (Input.GetKeyDown(HorDesKeys.crouch) && holdToCrouch)
            {
                isCrouched = false;

                var camPos = camPrefab.transform.position;

                camPrefab.transform.position = new Vector3(camPos.x, camPos.y - 3f, camPos.z);

                SpeedReduction();
            }
            else if (Input.GetKeyUp(HorDesKeys.crouch) && holdToCrouch)
            {
                isCrouched = true;

                var camPos = camPrefab.transform.position;

                camPrefab.transform.position = new Vector3(camPos.x, camPos.y + 3f, camPos.z);

                SpeedReduction();
            }
        }
    }

    public void CheckGround()
    {
        isGrounded = Physics.Raycast(mesh.transform.position, transform.TransformDirection(Vector3.down), .19f);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            _rigi.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        if (isCrouched && !holdToCrouch)
            SpeedReduction();
    }

    private void SpeedReduction()
    {
        if (isCrouched)
        {
            walkSpeed /= speedReduction;
            isCrouched = false;
        }
        else
        {
            walkSpeed *= speedReduction;
            isCrouched = true;
        }
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isGamePause = !isGamePause;
            return;
        }

        CursorState(isGamePause);
    }

    //Switches between linking the camera to different pivot points
    private void CameraHeadPivot()
    {
        if (isHeadPivot)
        {
            Transform cameraPivot = camPrefab.transform.parent;

            if(cameraPivot.parent.gameObject.name != "[PIVOT]")
                return;

            var headTransform = GameObject.FindGameObjectWithTag("Head").transform;
            cameraPivot.transform.SetParent(headTransform);
        }
        else
        {
            Transform cameraPivot = camPrefab.transform.parent;

            if(cameraPivot.parent.gameObject.tag != "Head")
                return;

            var players = GameObject.FindGameObjectsWithTag("Player");

            GameObject player = null;

            players.ToList().ForEach(x =>
            {
                if (x.GetComponent<NetworkingPlayerController>().isSelf)
                {
                    player = x;
                }
            });

            var pivot = player.transform.Find("[PIVOT]").transform;

            cameraPivot.transform.SetParent(pivot);
            camPrefab.transform.localRotation = Quaternion.identity;
        }
    }

    private void CursorState(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    public void LocalHide(bool value)
    {
        localHide.ForEach(x =>
        {
            x.SetActive(value);
        });
    }

    public static PhotonView GetLocalPlayer()
    {
        PhotonView[] views = FindObjectsOfType<PhotonView>();

        for(int i = 0; i < views.Length; i++)
        {
            if (views[i].IsMine)
            {
                return views[i];
            }
        }

        return new PhotonView();
    }
}