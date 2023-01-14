using Enemy;
using GameStateMashine;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private EnemyWave _enemyWave;
    [SerializeField] private GroundMovment _groundMovment;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemiesPrefab _enemiesPrefab;
    [SerializeField] private Player _player;
    [SerializeField] private GameStateSwitcher _gameStateSwitcher;
    [SerializeField] private BulletFactory _bulletFactory;

    public override void InstallBindings()
    {
        Container.Bind<BulletFactory>().FromComponentOn(_bulletFactory.gameObject).AsSingle();

        Container.Bind<GameStateSwitcher>().FromComponentOn(_gameStateSwitcher.gameObject).AsSingle();

        Container.Bind<Player>().FromComponentOn(_player.gameObject).AsSingle();

        Container.Bind<EnemiesPrefab>().FromScriptableObject(_enemiesPrefab).AsSingle();
        Container.Bind<EnemySpawner>().FromComponentOn(_enemySpawner.gameObject).AsSingle();
        Container.Bind<EnemyWave>().FromComponentOn(_enemyWave.gameObject).AsSingle();

        Container.Bind<GroundMovment>().FromComponentOn(_groundMovment.gameObject).AsSingle();
    }
}
