using System;
using System.Collections.Generic;
using System.Text;

public class SoundSystem
{
    public void PreEventLog(object sender, GameEventArgs e)
    {
        Console.WriteLine($"[Sound] 이벤트: {e.EventName}");
    }
}