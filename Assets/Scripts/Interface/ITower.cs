namespace TowerDefense
{
    public interface ITowerSetting
    {
        int AttackDamage { get; }
        float FireRate { get; }
        float AttackRange { get; }
    }
}