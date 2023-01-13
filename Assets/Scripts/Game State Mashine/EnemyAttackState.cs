using Enemy;
using System;
using UnityEngine;

namespace GameStateMashine
{
    class EnemyAttackState : BaseState
    {
        private EnemyWave _enemyWave;

        internal EnemyAttackState(EnemyWave enemyWave)
        {
            _enemyWave = enemyWave;
        }

        public override void Start()
        {
            _enemyWave.GoWave();
            _enemyWave.OnWaveFinished += FinishState;
            
        }

        public override void Stop()
        {
            _enemyWave.OnWaveFinished -= FinishState;
        }
    }
}
