using MovementSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class playerRunningState : PlayerMovingStates
    {
        private float startTime;

        private PlayerSprintData sprintData;
        public playerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationsData.RunParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;

            startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();
            if (!stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }
            if(Time.time < startTime * sprintData.RunToWalkTime)
            {
                return;
            }
            StopRunning();
        }
        #endregion


        #region Main Methods
        private void StopRunning()
        {
            if(stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.WalkingState);
        }
        #endregion


        #region Reusable Methods
        protected override void AddInputActionCallBacks()
        {
            base.AddInputActionCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        }


        protected override void RemoveInputActionCallBacks()
        {
            base.RemoveInputActionCallBacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        }
        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);
            base.OnMovementCanceled(context);
        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.WalkingState);
        }



        #endregion
    }
}
