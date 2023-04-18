using Enemy;
using UnityEngine;
using Zenject;

public class Gun : MonoBehaviour
{
    private Inventory _inventory;

    private BulletFactory _bulletFactory;

    private UIGun _currentGunData;

    [Inject]
    public void Construct(BulletFactory bulletFactory, Inventory inventory)
    {
        _bulletFactory = bulletFactory;
        _inventory = inventory;
    }

    private void OnEnable()
    {
        _inventory.OnGunLevelUped += TryChangeGun;
    }

    private void OnDisable()
    {
        _inventory.OnGunLevelUped -= TryChangeGun;
    }

    public void ShootTo(EnemyView enemy)
    {
        if (enemy == null)
            return;

        _bulletFactory.GetBullet().ShootToWithDamage(enemy.TargetForGun.transform.position, _currentGunData.Damage, transform.position);
    }

    private void TryChangeGun()
    {
        _currentGunData = _inventory.CurrentBestUIGun;
    }
}
