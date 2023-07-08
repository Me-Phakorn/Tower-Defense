namespace TowerDefense
{
    public interface ITowerSetting
    {
        int AttackDamage { get; }
        int AttackRange { get; }
        float FireRate { get; }
    }
}