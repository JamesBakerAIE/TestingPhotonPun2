using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Client.Player.CameraControls;

namespace Client.Player
{
    /// <summary>
    /// State for when the player is not performing any action.
    /// </summary>
    [RequireComponent(typeof(MovementStateMachine))]
    [RequireComponent(typeof(PlayerController))]
    public class IdleState : State, IPunObservable
    {
        #region Private Fields

        private MovementStateMachine stateMachine = null;
        private WalkState walkState = null;
        private JumpState jumpState = null;
        #endregion

        #region MonoBehaviour Callbacks
        public void Awake()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            stateMachine = GetComponent<MovementStateMachine>();
            walkState = GetComponent<WalkState>();
            jumpState = GetComponent<JumpState>();
        }
        #endregion

        #region Override Methods
        public override void Enter()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        public override void HandleInput()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if(jumpAction.triggered && player.characterController.isGrounded)
            {
                jump = true;
            }

            if (moveAction.triggered)
            {
                move = true;
            }
        }

        public override void LogicUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (move)
            {
                move = false;
                stateMachine.ChangeState(walkState);
            }
            else if(jump)
            {
                jump = false;
                stateMachine.ChangeState(jumpState);
            }

            if(player != null)
            {
                player.gravityVelocity += Physics.gravity * player.gravityMultiplier * Time.deltaTime;

                if (player.gravityVelocity.y < player.minFallDistance && player.characterController.isGrounded && player.enableCameraShake)
                {
                    CameraShake.Instance.ShakeCamera(player.landingCameraShakeInstensity, player.landingCameraShakeTime);
                }

                if (player.characterController.isGrounded && player.gravityVelocity.y < 0)
                {
                    player.gravityVelocity.y = -2f;
                }

                player.characterController.Move(player.gravityVelocity * Time.deltaTime);


            }



        }

        public override void PhysicsUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        public override void Exit()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        #endregion

        #region IPunObservable Implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        #endregion
    }

}

