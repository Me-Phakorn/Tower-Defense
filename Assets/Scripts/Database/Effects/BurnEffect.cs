
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Burn Effect", menuName = "Tower-Defense/Effect/Burn", order = 0)]
    public class BurnEffect : Effect
    {
        [SerializeField, Range(1, 10)]
        protected int effectTick = 3;

        public override void ApplyEffect(float baseDamage)
        {

        }
    }
}