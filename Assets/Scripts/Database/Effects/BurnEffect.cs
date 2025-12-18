
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Burn Effect", menuName = "Tower-Defense/Effect/Burn", order = 0)]
    public class BurnEffect : Effect
    {
        [SerializeField, Range(1, 10)]
        protected int effectTick = 3;

        public int EffectTick => effectTick;
        public float TickInterval => effectDuration / effectTick;
        
        public float GetTickDamage(float baseDamage, int stackCount)
        {
            return baseDamage * (effectPercent / 100f) * stackCount;
        }

        public override void ApplyEffect(float baseDamage, Enemy[] enemies)
        {
            foreach (IEffectStatus enemy in enemies)
                enemy.AddEffectStatus(baseDamage, this);
        }
    }
}