namespace TowerDefense
{
    public enum EnemyType
    {
        Speedster,
        Minion,
        Giant
    }

    public enum EffectType
    {
        Slow,
        Burn
    }

    public enum ConditionType
    {
        EveryTurn,
        EveryTime
    }

    public enum TowerAimType
    {
        Nearest,
        Farthest,
        Nearest_Less_Health,
        Farthest_Less_Health,
        Nearest_Most_Health,
        Farthest_Most_Health,
    }
}