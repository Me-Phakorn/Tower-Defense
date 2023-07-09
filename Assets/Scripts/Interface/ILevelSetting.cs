namespace TowerDefense.Setting
{
    public interface ILevelSetting
    {
        bool IsLose { get; }
        bool IsPause { get; }

        float GameSpeed { get; }

        float GameTime { get; set; }
        int GameWave { get; set; }
    }
}