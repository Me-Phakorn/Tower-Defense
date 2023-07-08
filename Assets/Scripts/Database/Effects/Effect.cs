
using UnityEngine;

namespace TowerDefense.Database
{
    public abstract class Effect : ScriptableObject, IEffectSetting
    {
        [System.Serializable]
        public class Stack
        {
            public Effect effect;

            public float baseDamage;
            public float effectTimer;
            public float effectDuration;

            public Stack(float baseDamage, Effect effect)
            {
                this.effect = effect;
                this.baseDamage = baseDamage;

                effectDuration = effect.EffectDuration;

                effectTimer = 0;
            }
        }

        [Header("General")]
        public string effectName;
        [TextArea] public string description;

        [Header("Effect Setting")]
        [SerializeField, Range(0, 300)]
        protected float effectPercent = 50;
        [SerializeField, Range(0.5f, 5)]
        protected float effectDuration = 2;

        public float EffectPercent => effectPercent;
        public float EffectDuration => effectDuration;

        public abstract void ApplyEffect(float baseDamage, Enemy[] enemies);
    }
}