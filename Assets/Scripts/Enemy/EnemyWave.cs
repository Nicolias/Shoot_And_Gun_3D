using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyWave : MonoBehaviour
    {
        public event Action OnWaveFinished;

        private List<EnemyView> _currentEnemies;

        private EnemySpawner _enemySpawner;
        private EnemiesPrefab _enemiesPrefab;

        private EnemyFactory _enemyFactory;

        [Inject]
        public void Construct(EnemiesPrefab enemiesPrefab, EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
            _enemiesPrefab = enemiesPrefab;
        }

        private void Awake()
        {
            _enemyFactory = new(_enemiesPrefab, _enemySpawner);
        }

        public void GoWave()
        {
            _currentEnemies = _enemyFactory.SpawnEnemies();

            StartCoroutine(WaitUntilAllEnemiesDead());
        }

        private IEnumerator WaitUntilAllEnemiesDead()
        {
            for (int i = 0; i < _currentEnemies.Count; i++)
            {
                if(_currentEnemies[i] != null)
                    yield return new WaitUntil(() => _currentEnemies[i].IsDead);
            }

            OnWaveFinished?.Invoke();
        }
    }
}
