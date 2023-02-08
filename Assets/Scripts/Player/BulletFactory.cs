using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private Bullet _bulletsTemplate;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private int _bulletCountInPool;

    private Queue<Bullet> _bulletsPool = new();
    private Bullet _currentBullet;

    private void Start()
    {
        for (int i = 0; i < _bulletCountInPool; i++)
        {
            Bullet bullet = Instantiate(_bulletsTemplate, _bulletSpawn);
            bullet.gameObject.SetActive(false);

            _bulletsPool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (_currentBullet != null)
            _bulletsPool.Enqueue(_currentBullet);

        _currentBullet = _bulletsPool.Dequeue();

        return _currentBullet;
    }
}
