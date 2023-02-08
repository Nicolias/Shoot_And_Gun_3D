using Enemy;
using GameStateMashine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private int _health;
    [SerializeField] private Slider _healthSlider;

    [SerializeField] private Gun _gun;

    [SerializeField] private LayerMask _enemyLayer;
    private GameStateSwitcher _gameStateSwitcher;
    private EnemyView _currentEnemy;

    [SerializeField] private float _attackCoolDown;
    private float _nextAttackTime;

    private DamageUI _damageUI;

    private bool _canShooting;

    [Inject]
    public void Construct(GameStateSwitcher gameStateSwitcher, DamageUI damageUI)
    {
        _damageUI = damageUI;

        _gameStateSwitcher = gameStateSwitcher;
        _nextAttackTime = _attackCoolDown;

        _healthSlider.maxValue = _health;
        _healthSlider.value = _health;
        _healthSlider.minValue = 0;
    }

    private void OnEnable()
    {
        _gameStateSwitcher.OnStateChanged += ValidateShootState;

        _canShooting = true;
        ValidateShootState();
    }

    private void OnDisable()
    {
        _gameStateSwitcher.OnStateChanged -= ValidateShootState;
    }

    private void Update()
    {
        if (_canShooting && _nextAttackTime < Time.time)
        {
            ShootToClosestEnemy();

            _nextAttackTime = Time.time + _attackCoolDown;
        }
    }

    public void ApplayDamage(int damage)
    {
        if (damage <= 0) throw new System.ArgumentOutOfRangeException("Отрицательный урон");

        _health -= damage;

        _damageUI.AddText(damage, transform.position, Color.yellow);

        _healthSlider.value = _health;
    }

    private void ShootToClosestEnemy()
    {
        var closestEnemy = FindClosestEnemy();

        _gun.ShootTo(closestEnemy);
        if(closestEnemy != null)
            transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
    }

    private EnemyView FindClosestEnemy()
    {
        if (_currentEnemy != null)
            if (_currentEnemy.IsDead == false)
                return _currentEnemy;

        List<EnemyView> enemies = FindEnemies();

        EnemyView nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        float distance;

        foreach (var enemy in enemies)
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        _currentEnemy = nearestEnemy;

        return nearestEnemy;
    }

    private List<EnemyView> FindEnemies()
    {
        int radiosToFindEnemy = 10;
        Collider[] hitColliders = new Collider[10];

        List<EnemyView> enemies = new();

        int recordsMade = Physics.OverlapSphereNonAlloc(
            transform.position,
            radiosToFindEnemy,
            hitColliders,
            _enemyLayer);

        for (int i = 0; i < recordsMade; i++)
            if (hitColliders[i].TryGetComponent(out EnemyView enemy))
                enemies.Add(enemy);

        return enemies;
    }

    private void ValidateShootState()
    {
        if(_gameStateSwitcher.CurrentState != null)
            _canShooting = _gameStateSwitcher.CurrentState is EnemyAttackState;

        _animator.SetBool("CanShooting", _canShooting);
    }
}