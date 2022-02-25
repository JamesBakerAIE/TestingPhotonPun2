using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Client.Player
{
    /// <summary>
    /// Base class for player state machines.
    /// </summary>
    public class State : MonoBehaviourPun
    {
        #region Hidden Fields

        [HideInInspector]
        public InputAction moveAction;

        [HideInInspector]
        public InputAction jumpAction;

        [HideInInspector]
        public PlayerController player;

        [HideInInspector]
        public bool move;

        [HideInInspector]
        public bool jump;

        [HideInInspector]
        public Vector2 moveInput = Vector2.zero;
     
        [HideInInspector]
        public Vector3 velocity = Vector3.zero;

        #endregion

        #region Protected Fields



        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            player = GetComponent<PlayerController>();

            moveAction = player.playerInput.actions["Move"];
            jumpAction = player.playerInput.actions["Jump"];
        }

        #endregion

        #region Virtual Methods
        public virtual void Enter()
        {

        }

        public virtual void HandleInput()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void DelayedUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }

        #endregion
    }

}
