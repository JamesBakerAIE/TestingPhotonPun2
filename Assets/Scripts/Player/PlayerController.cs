using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Client.Player  
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        #region Camera Settings
        [Header("Camera Objects")]

        [Tooltip("Player's virtual camera")]
        [SerializeField]
        CinemachineVirtualCamera virtualCamera = null;

        [Tooltip("Object that the virtual camera should follow.")]
        [SerializeField]
        private GameObject followTarget = null;

        [Tooltip("Player's camera")]
        public Camera cam = null;

        [Tooltip("Should the camera shake?")]
        public bool enableCameraShake = true;

        [Tooltip("The amount of time (in seconds) the camera shake (when landing) will last for.")]
        public float landingCameraShakeTime = 0.1f;

        [Tooltip("The intensity of the camera shake when landing.")]
        public float landingCameraShakeInstensity = 5.0f;

        #endregion

        #region Static Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Player Settings

        [Header("Player Settings")]

        [Tooltip("Player's walk speed.")]
        public float walkSpeed = 5;

        [Tooltip("Player's jump height.")]
        public float jumpHeight = 3;

        [Tooltip("Used to calculate gravity applied to the player by calculating: Physics.gravity *= gravityMultiplier.")]
        public float gravityMultiplier = 1.5f;

        [Tooltip("The distance that the player starts shaking the camera when landing on the ground.")]
        public float minFallDistance = -10;


        #endregion

        #region Hidden Fields

        [HideInInspector]
        public PlayerInput playerInput = null;

        [HideInInspector]
        public Vector3 playerVelocity;

        [HideInInspector]
        public Vector3 gravityVelocity = Vector3.zero;

        [HideInInspector]
        public CharacterController characterController = null;

        #endregion

        #region Private Fields

        private MovementStateMachine stateMachine = null;
        private IdleState idleState = null;
        private WalkState walkState = null;

        #endregion

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if(photonView.IsMine)
            {
                PlayerController.LocalPlayerInstance = this.gameObject;
                playerInput = GetComponent<PlayerInput>();
                characterController = GetComponent<CharacterController>();
                stateMachine = GetComponent<MovementStateMachine>();
                idleState = GetComponent<IdleState>();
                walkState = GetComponent<WalkState>();
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            if(photonView.IsMine == false)
            {
                return;
            }

            stateMachine.Initialize(idleState);

            virtualCamera.gameObject.SetActive(true);
            cam.gameObject.SetActive(true);

            virtualCamera.Follow = followTarget.transform;
        }

        private void Update()
        {
            if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            stateMachine.currentState.HandleInput();
            stateMachine.currentState.LogicUpdate();

            //float inputX = Input.GetAxisRaw("Horizontal");
            ////float inputY = Input.GetAxisRaw("Vertical");

            //Vector3 move = new Vector3(inputX, 0, 0);

            //characterController.Move(move * walkSpeed * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            stateMachine.currentState.DelayedUpdate();
        }
        private void FixedUpdate()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            stateMachine.currentState.PhysicsUpdate();
        }

        #endregion

    }

}

