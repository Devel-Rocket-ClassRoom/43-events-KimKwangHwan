using System;
using System.Collections.Generic;
using System.Text;

public class ScoreSystem
{
    public void ScoreChanged(object sender, GameEventArgs e)
    {
        if (e.EventName == "ScoreChanged")
        {
            Console.WriteLine($"점수 변경: {(int)(e.Data)}점");
        }
    }
}