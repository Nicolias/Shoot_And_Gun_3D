using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;

        private Player _player;

        private BaseEnemyModel _enemyModel;

        private bool _isMoving;

        private const float _attackDuration = 1f;
        private const float _attackDistance = 1.5f;
        private const int _speed = 1;

        private float _nextAttackTime;

        public bool IsDead { get; set; }

        [Inject]
        public void Constract(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            _isMoving = true;
        }

        private void Update()
        {
            if (_isMoving == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, _player.transform.position) <= _attackDistance
                    && Time.time >= _nextAttackTime)
                {
                    _isMoving = false;

                    Attack();
                    _nextAttackTime = Time.time + _attackDuration;
                }
            }
            else 
            {
                if (Vector2.Distance(transform.position, _player.transform.position) > _attackDistance)
                    _isMoving = true;
            }
        }

        public void ApplayDamage(int damage)
        {
            _enemyModel.ApplayDamage(damage);

            if (_enemyModel.Health <= 0)
            {
                Destroy(gameObject);
                IsDead = true;
            }

            _healthBar.value = _enemyModel.Health;
        }

        internal void SetModel(BaseEnemyModel enemyModel)
        {
            _healthBar.maxValue = enemyModel.Health;
            _healthBar.value = enemyModel.Health;
            _healthBar.minValue = 0;

            _enemyModel = enemyModel;
        }

        private void Attack()
        {
            _player.ApplayDamage(_enemyModel.Damage);
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}
