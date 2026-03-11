using System;

public class Program
{
    public static void Main()
    {
        ScoreSystem sc = new ScoreSystem();
        AchievementSystem ac = new AchievementSystem();
        SoundSystem ss = new SoundSystem();

        EventManager.OnGameEvent += ss.PreEventLog;
        EventManager.OnGameEvent += sc.ScoreChanged;
        EventManager.OnGameEvent += ac.Achievement;

        EventManager.TriggerEvent("ScoreChanged", 100);
        EventManager.TriggerEvent("Achievement", "첫 번째 적 처치");
        EventManager.TriggerEvent("GameOver");
    }
}
