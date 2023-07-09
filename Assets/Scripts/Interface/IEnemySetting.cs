namespace TowerDefense
{
    public interface IEnemySetting
    {
        EnemyType Type { get; }

        int AttackDamage { get; }

        float BaseHealth { set; get; }
        float BaseSpeed { set; get; }
    }
}