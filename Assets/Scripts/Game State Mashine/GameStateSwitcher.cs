using Enemy;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;
using System;

namespace GameStateMashine
{
    public class GameStateSwitcher : MonoBehaviour
    {
        public event Action OnStateChanged;

        private GroundMovment _groundMovment;
        private EnemyWave _enemyWave;

        private Queue<BaseState> _gameStates;
        private BaseState _currentState;

        public BaseState CurrentState => _currentState;

        [Inject]
        public void Construct(GroundMovment groundMovment, EnemyWave enemyWave)
        {
            _groundMovment = groundMovment;
            _enemyWave = enemyWave;
        }

        private void Awake()
        {
            _gameStates = new Queue<BaseState>();
            _gameStates.Enqueue(new EnemyAttackState(_enemyWave));
            _gameStates.Enqueue(new ChangeStageState(_groundMovment));

            GoToNextState();
        }

        private void OnEnable()
        {
            CurrentState.OnFinished += InvokeEventChangeState;
        }

        private void OnDisable()
        {
            CurrentState.OnFinished -= InvokeEventChangeState;
        }

        private void InvokeEventChangeState()
        {
            OnStateChanged?.Invoke();
        }

        private void GoToNextState()
        {
            if (_currentState != null)
            {
                _gameStates.Enqueue(_currentState);

                _currentState.Stop();
                _currentState.OnFinished -= GoToNextState;
                CurrentState.OnFinished -= InvokeEventChangeState;
            }

            if (_gameStates.Any() == false)
                return;

            _currentState = _gameStates.Dequeue();
            _currentState.OnFinished += GoToNextState;
            CurrentState.OnFinished += InvokeEventChangeState;
            _currentState.Start();
        }
    }
}
