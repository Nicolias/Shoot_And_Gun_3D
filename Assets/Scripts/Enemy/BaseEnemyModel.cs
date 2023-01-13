namespace Enemy
{
    internal class BaseEnemyModel
    {
        private readonly int _damage;
        private int _health;

        internal int Health => _health;
        internal int Damage => _damage;

        internal BaseEnemyModel(int health, int damage)
        {
            _damage = damage;
            _health = health;
        }

        internal void ApplayDamage(int damage)
        {
            if (damage <= 0) throw new System.ArgumentOutOfRangeException("Отрицательный урон");

            _health -= damage;
        }
    }
}
