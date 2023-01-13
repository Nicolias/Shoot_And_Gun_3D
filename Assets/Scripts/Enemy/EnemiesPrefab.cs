using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemies")]
    public class EnemiesPrefab : ScriptableObject
    {
        [SerializeField] private List<EnemyView> _enemiesVariation;
        public List<EnemyView> EnemiesVariation => _enemiesVariation;
    }
}