using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions;
    [SerializeField] private float _distanceBetweenPlayerAndEnemies;

    private DiContainer _diContainer;
    private Player _player;

    private List<EnemyView> _enemyViews = new();
    public List<EnemyView> CurrentEnemies => _enemyViews;

    [Inject]
    public void Construct(Player player, DiContainer diContainer)
    {
        _player = player;
        _diContainer = diContainer;
    }

    public void SpawnEnemies(List<EnemyView> enemies)
    {
        _enemyViews.Clear();
        StartCoroutine(SpawnEnemiesAfterEverySeconds(enemies, 1.5f));
    }

    private IEnumerator SpawnEnemiesAfterEverySeconds(List<EnemyView> enemies, float seconds)
    {
        CorrectedSpawnPosition();

        foreach (var enemy in enemies)
        {
            EnemyView currentEnemy = _diContainer.InstantiatePrefabForComponent<EnemyView>(enemy);
            _enemyViews.Add(currentEnemy);
        }

        foreach (var currentEnemy in _enemyViews)
        {
            currentEnemy.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;

            yield return new WaitForSeconds(seconds);
        }
    }

    private void CorrectedSpawnPosition()
    {
        foreach (var spawnPosition in _spawnPositions)
        {
            var correctingPosition = spawnPosition.position;
            correctingPosition.x = _player.transform.position.x - _distanceBetweenPlayerAndEnemies;
            spawnPosition.position = correctingPosition;
        }
    }
}
