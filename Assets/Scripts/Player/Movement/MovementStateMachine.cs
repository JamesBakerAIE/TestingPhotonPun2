using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Player
{
    /// <summary>
    /// State machine for player movement.
    /// </summary>
    public class MovementStateMachine : MonoBehaviour
    {
        #region Public Fields

        public State currentState { get; private set; }

        #endregion

        #region Public Methods
        public void Initialize(State startingState)
        {
            currentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            currentState.Exit();

            currentState = newState;
            newState.Enter();
        }

        #endregion
    }

}

