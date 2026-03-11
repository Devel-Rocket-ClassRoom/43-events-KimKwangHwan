using System;
using System.Collections.Generic;
using System.Text;

public class AchievementSystem
{
    public void Achievement(object sender, GameEventArgs e)
    {
        if (e.EventName == "Achievement")
        {
            Console.WriteLine($"업적 달성: {(string)(e.Data)}");
        }
    }
}