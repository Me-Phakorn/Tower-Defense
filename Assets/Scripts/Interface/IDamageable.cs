namespace TowerDefense
{
    public interface IDamageable
    {
        float Damages { get; }

        void Damage(float amount);
    }
}