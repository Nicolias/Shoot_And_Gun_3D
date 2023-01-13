using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyFactory
    {
        private EnemiesPrefab _enemiesPrefab;
        private EnemySpawner _enemySpawner;

        private int _currentWave;

        public EnemyFactory(EnemiesPrefab enemiesPrefab, EnemySpawner enemySpawner)
        {
            _enemiesPrefab = enemiesPrefab;
            _enemySpawner = enemySpawner;
        }

        internal List<EnemyView> SpawnEnemies()
        {
            _currentWave++;

            _enemySpawner.SpawnEnemies(SelecteEnemyPrefabs());

            ConnectModeleWithViwe();

            return _enemySpawner.CurrentEnemies;
        }

        private List<EnemyView> SelecteEnemyPrefabs()
        {
            List<EnemyView> generatedEnemy = new();
            for (int i = 0; i < Random.Range(1, 5); i++)
                generatedEnemy.Add(_enemiesPrefab.EnemiesVariation[Random.Range(0, _enemiesPrefab.EnemiesVariation.Count)]);

            return generatedEnemy;
        }

        private void ConnectModeleWithViwe()
        {
            for (int i = 0; i < _enemySpawner.CurrentEnemies.Count; i++)
            {
                _enemySpawner.CurrentEnemies[i].SetModel(new BaseEnemyModel(_currentWave * 10, _currentWave * 10));
            }
        }
    }
}
