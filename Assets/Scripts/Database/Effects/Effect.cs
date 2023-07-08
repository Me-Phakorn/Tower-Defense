
using UnityEngine;

namespace TowerDefense.Database
{
    public abstract class Effect : ScriptableObject
    {
        [Header("General")]
        public string effectName;
        [TextArea] public string description;

        [Header("Effect Setting")]
        [SerializeField, Range(0, 300)]
        protected float damagePercent = 50;
        [SerializeField, Range(0.5f, 5)]
        protected float effectDuration = 2;

        public abstract void ApplyEffect(float baseDamage , Enemy[] enemies);
    }
}