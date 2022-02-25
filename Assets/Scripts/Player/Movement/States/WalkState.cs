using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Player;
using Photon.Pun;
using Client.Player.CameraControls;

namespace Client.Player
{
    /// <summary>
    /// State for when the player is walking.
    /// </summary>
    [RequireComponent(typeof(MovementStateMachine))]
    [RequireComponent(typeof(PlayerController))]
    public class WalkState : State, IPunObservable
    {
        #region Private Fields

        private MovementStateMachine stateMachine = null;
        private IdleState idleState = null;
        private JumpState jumpState = null;

        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            stateMachine = GetComponent<MovementStateMachine>();
            idleState = GetComponent<IdleState>();
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

            moveInput = moveAction.ReadValue<Vector2>();

            if(jumpAction.triggered && player.characterController.isGrounded)
            {
                jump = true;
            }

            velocity = new Vector3(moveInput.x, 0, moveInput.y);
        }

        public override void LogicUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            velocity = new Vector3(moveInput.x, 0, moveInput.y);

            velocity = velocity.x * player.cam.transform.right.normalized;

            player.gravityVelocity += Physics.gravity * player.gravityMultiplier * Time.deltaTime;

            if (player.gravityVelocity.y < player.minFallDistance && player.characterController.isGrounded && player.enableCameraShake)
            {
                CameraShake.Instance.ShakeCamera(player.landingCameraShakeInstensity, player.landingCameraShakeTime);
            }



            if (player.characterController.isGrounded && player.gravityVelocity.y < 0)
            {
                player.gravityVelocity.y = -2f;
            }

            player.characterController.Move(velocity * Time.deltaTime * player.walkSpeed + player.gravityVelocity * Time.deltaTime);

            if (moveInput == Vector2.zero)
            {
                stateMachine.ChangeState(idleState);
            }
            else if(jump)
            {
                jump = false;
                stateMachine.ChangeState(jumpState);
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
