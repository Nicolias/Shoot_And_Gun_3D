using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Animator _animator;

        [SerializeField] private float _speed;

        private DamageUI _damageUI;
        private Player _player;
        private CreditCounter _creditCounter;

        private BaseEnemyModel _enemyModel;

        [SerializeField] private float _attackDuration;
        [SerializeField] private float _attackDistance;

        private float _nextAttackTime;

        private float _previosPositionByX;

        public bool IsDead { get; set; }

        [Inject]
        public void Constract(Player player, DamageUI damageUI, CreditCounter creditCounter)
        {
            _player = player;
            _damageUI = damageUI;
            _creditCounter = creditCounter;
        }

        private void Start()
        {
            _previosPositionByX = transform.position.x;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, _player.transform.position) <= _attackDistance)
            {
                if (Time.time < _nextAttackTime)
                    return;

                StartCoroutine(Attack());
                _nextAttackTime = Time.time + _attackDuration;

                _animator.SetBool("IsAttack", true);
            }
            else
            {
                transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime * _speed),
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player.transform.position - transform.position), 1));

                MoveAnimation();
            }
        }

        public void ApplayDamage(int damage)
        {
            _enemyModel.ApplayDamage(damage);

            _damageUI.AddText(damage, transform.position, Color.red);

            if (_enemyModel.Health <= 0)
            {
                _creditCounter.AddMoney(25);
                Destroy(gameObject);
                IsDead = true;
            }

            _healthBar.value = _enemyModel.Health;
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        internal void SetModel(BaseEnemyModel enemyModel)
        {
            _healthBar.maxValue = enemyModel.Health;
            _healthBar.value = enemyModel.Health;
            _healthBar.minValue = 0;

            _enemyModel = enemyModel;
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(0.46f);

            _player.ApplayDamage(_enemyModel.Damage);
        }

        private void MoveAnimation()
        {
            var currentPosition = transform.position.x;
            _animator.SetBool("IsAttack", false);
            _animator.SetFloat("Speed", currentPosition - _previosPositionByX);

            _previosPositionByX = transform.position.x;
        }
    }
}
