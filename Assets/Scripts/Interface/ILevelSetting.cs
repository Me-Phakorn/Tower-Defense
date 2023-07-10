namespace TowerDefense.Setting
{
    public interface ILevelSetting
    {
        bool IsLose { get; }
        bool IsPause { get; }
        int Health { get; }
        int Money { get; }

        float GameSpeed { get; }

        float GameTime { get; set; }
        int GameWave { get; set; }
    }
}