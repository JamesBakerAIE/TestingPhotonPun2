using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Client.Player.CameraControls;

namespace Client.Player
{
    public class JumpState : State, IPunObservable
    {
        #region Private Fields

        private MovementStateMachine stateMachine = null;
        private IdleState idleState = null;
        private WalkState walkState = null;

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
            walkState = GetComponent<WalkState>();
        }

        #endregion

        #region Override Methods
        public override void Enter()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            player.gravityVelocity.y = 0;
            player.gravityVelocity.y += Mathf.Sqrt(player.jumpHeight * -3.0f * Physics.gravity.y * player.gravityMultiplier);
        }

        public override void HandleInput()
        {
            if (!photonView.IsMine)
            {
                return;
            }


            moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                move = true;
            }

            velocity = new Vector3(moveInput.x, 0, moveInput.y);


            velocity = new Vector3(moveInput.x, 0, moveInput.y);

            velocity = velocity.x * player.cam.transform.right.normalized;

            player.gravityVelocity += Physics.gravity * player.gravityMultiplier * Time.deltaTime;

            if (player.characterController.isGrounded && player.gravityVelocity.y < 0)
            {
                player.gravityVelocity.y = 0f;
            }

            player.characterController.Move(velocity * Time.deltaTime * player.walkSpeed + player.gravityVelocity * Time.deltaTime);
        }

        public override void LogicUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (move && player.characterController.isGrounded)
            {   
                //CameraShake.Instance.ShakeCamera(5f, 0.1f);
                stateMachine.ChangeState(walkState);
                
            }
            else if (player.characterController.isGrounded)
            {
               // CameraShake.Instance.ShakeCamera(5f, 0.1f);
                stateMachine.ChangeState(idleState);
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        #endregion
    }

}
