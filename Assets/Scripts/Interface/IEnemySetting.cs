namespace TowerDefense
{
    public interface IEnemySetting
    {
        int AttackDamage { get; }

        float BaseHealth { get; }
        float BaseSpeed { get; }
    }
}