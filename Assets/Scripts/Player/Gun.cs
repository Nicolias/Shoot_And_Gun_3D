
using System.Collections.Generic;
using UnityEngine;

public class Gun
{

}

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private Queue<Bullet> _bulletsPool;
    private Bullet _currentBullet;

    public Bullet GetBullet()
    {
        _currentBullet = _bulletsPool.Dequeue();
        _bulletsPool.Enqueue(_currentBullet);
        return _currentBullet;
    }
}

public class Bullet : MonoBehaviour
{
    private int _damage;
    public int Damage => _damage;
}