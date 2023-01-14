using Enemy;
using UnityEngine;
using Zenject;

public class Gun : MonoBehaviour
{
    [SerializeField] private int _damage;

    private BulletFactory _bulletFactory;

    [Inject]
    public void Construct(BulletFactory bulletFactory)
    {
        _bulletFactory = bulletFactory;
    }

    public void ShootTo(EnemyView enemy)
    {
        if (enemy == null)
            return;

        _bulletFactory.GetBullet().ShootToWithDamage(enemy.transform.position, _damage, transform.position);
    }
}
