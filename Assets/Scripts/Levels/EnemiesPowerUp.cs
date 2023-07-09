using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TowerDefense.Setting
{
    [CreateAssetMenu(fileName = "Enemies Power Up Condition", menuName = "Tower-Defense/Condition/Power Up", order = 0)]
    public class EnemiesPowerUp : LevelCondition
    {
        [Header("Condition")]
        [SerializeField] private ConditionType conditionType;

        [SerializeField, Range(0.01f, 100f)]
        private float powerUpPercent = 0.5f;

        public override IEnemySetting GetEnemySetting(ILevelSetting levelSetting, Enemy enemy)
        {
            var _enemy = enemyCollection.Collections.Where(e => e.Enemy == enemy).FirstOrDefault();

            IEnemySetting _newSetting = _enemy as IEnemySetting;

            switch (conditionType)
            {
                case ConditionType.EveryTurn:
                    {
                        if (levelSetting.GameWave - 1 > 0)
                        {
                            _newSetting.BaseHealth = _enemy.BaseHealth + (_enemy.BaseHealth * (powerUpPercent / 100)) * (levelSetting.GameWave - 1);
                            _newSetting.BaseSpeed = _enemy.BaseSpeed + (_enemy.BaseSpeed * (powerUpPercent / 100)) * (levelSetting.GameWave - 1);
                        }

                        return _newSetting;
                    }
                case ConditionType.EveryTime:
                    {
                        break;
                    }
                default:
                    return null;
            }

            return null;
        }
    }
}