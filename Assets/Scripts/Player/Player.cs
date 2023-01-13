using Enemy;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Gun _gun;

    public void ApplayDamage(int damage)
    {
        if (damage <= 0) throw new System.ArgumentOutOfRangeException("Отрицательный урон");

        _health -= damage;
    }

    private void ShootToClosestEnemy()
    {
        
    }

    private EnemyView FindClosestEnemy()
    {
        return null;
    }
}

