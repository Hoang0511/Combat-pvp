using MovementSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerSprintingState : PlayerMovingStates
    {
        private PlayerSprintData sprintData;

        private float startTime;
        private bool keepSprinting;
        private bool shouldResetSprintState;
        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationsData.SprintParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            startTime = Time.time;

            shouldResetSprintState = true;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.SprintParameterHash);

            if (shouldResetSprintState )
            {
                keepSprinting = false;

                stateMachine.ReusableData.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();
            if(keepSprinting)
            {
                return;
            }
            if(Time.time <startTime + sprintData.SprintToRunTime)
            {
                return;
            }
            StopSprinting();
        }


        #endregion


        #region Main Methods
        private void StopSprinting()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();
            stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        

        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }

        protected override void OnFall()
        {
            shouldResetSprintState = false;
            base.OnFall();
        }

        #endregion

        #region Input Methods


        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;
            base.OnJumpStarted(context);

        }
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);
            base.OnMovementCanceled(context);
        }
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
            stateMachine.ReusableData.ShouldSprint = true;
        }
        #endregion
    }
}
