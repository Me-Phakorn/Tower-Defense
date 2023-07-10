namespace TowerDefense
{
    public interface ITowerSetting
    {
        EnemyType TargetType { get; }
        TowerAimType AimType { get; }

        int TowerPrice { get; }

        int AttackDamage { get; }
        int AttackRange { get; }
        float FireRate { get; }
    }
}