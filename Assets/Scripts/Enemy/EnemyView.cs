using System.Collections;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        private Player _player;

        private BaseEnemyModel _enemyModel;

        private bool _isMoving;
        private int _speed = 1;

        private float _nextAttackTime;
        private float _attackDuration = 1f;

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

                if (Vector2.Distance(transform.position, _player.transform.position) < 0.5
                    && Time.time >= _nextAttackTime)
                {
                    Attack();
                    _nextAttackTime = Time.time + _attackDuration;
                }
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
        }

        internal void SetModel(BaseEnemyModel enemyModel)
        {
            _enemyModel = enemyModel;
        }

        private void Attack()
        {
            _player.ApplayDamage(_enemyModel.Damage);
        }
    }
}
