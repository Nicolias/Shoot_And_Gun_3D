using System;

namespace GameStateMashine
{
    class ChangeStageState : BaseState
    {
        private GroundMovment _groundMovment;

        public ChangeStageState(GroundMovment groundMovment)
        {
            _groundMovment = groundMovment;
        }

        public override void Start()
        {
            _groundMovment.StartCoroutine(_groundMovment.MoveToNextStage());
            _groundMovment.OnStageChanged += FinishState;
        }

        public override void Stop()
        {
            _groundMovment.OnStageChanged -= FinishState;
        }        
    }
}
