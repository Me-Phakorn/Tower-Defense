
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Slow Effect", menuName = "Tower-Defense/Effect/Slow", order = 0)]
    public class SlowEffect : Effect
    {
        [SerializeField, Range(1, 100)]
        protected float slowPercent = 35;

        public override void ApplyEffect(float baseDamage, Enemy[] enemies)
        {

        }
    }
}