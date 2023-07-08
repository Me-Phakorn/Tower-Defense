namespace TowerDefense
{
    public interface IEffectStatus
    {
        void AddEffectStatus<T>(float baseDamage, T effect) where T : Database.Effect;
    }
}