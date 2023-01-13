using Enemy;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;

namespace GameStateMashine
{
    public class GameStateSwitcher : MonoBehaviour
    {
        private GroundMovment _groundMovment;
        private EnemyWave _enemyWave;

        private BaseState _currentState;
        private Queue<BaseState> _gameStates;

        [Inject]
        public void Construct(GroundMovment groundMovment, EnemyWave enemyWave)
        {
            _groundMovment = groundMovment;
            _enemyWave = enemyWave;
        }

        private void Start()
        {
            _gameStates = new Queue<BaseState>();
            _gameStates.Enqueue(new EnemyAttackState(_enemyWave));
            _gameStates.Enqueue(new ChangeStageState(_groundMovment));

            GoToNextState();
        }

        private void GoToNextState()
        {
            if (_currentState != null)
            {
                _gameStates.Enqueue(_currentState);

                _currentState.Stop();
                _currentState.OnFinished -= GoToNextState;
            }

            if (_gameStates.Any() == false)
                return;

            _currentState = _gameStates.Dequeue();
            _currentState.OnFinished += GoToNextState;
            _currentState.Start();
        }
    }
}
