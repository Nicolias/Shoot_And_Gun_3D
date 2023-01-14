using Enemy;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _speed;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _spawnPosition;

    private bool _isMoving;
    private Vector3 _moveDiraction;

    private int _damage;
    public int Damage => _damage;

    private void OnEnable()
    {
        _isMoving = false;
    }

    private void Update()
    {
        if (_isMoving == true)
            transform.position += (_moveDiraction - _spawnPosition).normalized * _speed * Time.deltaTime;
    }

    public void ShootToWithDamage(Vector3 dirrection, int damage, Vector3 spawnPosition)
    {
        transform.position = spawnPosition;

        _spawnPosition = spawnPosition;

        gameObject.SetActive(true);

        _moveDiraction = dirrection;
        _isMoving = true;
        _damage = damage;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyView enemy))
        {
            enemy.ApplayDamage(_damage);
            gameObject.SetActive(false);
            transform.position = _spawnPosition;
        }
    }
}