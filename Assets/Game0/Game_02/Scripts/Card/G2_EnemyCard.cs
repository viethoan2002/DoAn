using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    public class G2_EnemyCard : G2_BaseCard
    {
        [FormerlySerializedAs("enemyStats")] [SerializeField] private G2_EnemyStats g2EnemyStats;
    }
}
