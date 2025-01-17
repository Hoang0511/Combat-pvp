using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationsData.MediumStopParameterHash);

            stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.MediumDecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.MediumStopParameterHash);

        }

        #endregion
    }
}
