using System;
using System.Collections.Generic;
using System.Text;

public class GameEventArgs : EventArgs
{
    public string EventName { get; set; }
    public object Data { get; set; }

    public GameEventArgs(string eventName, object data)
    {
        EventName = eventName; Data = data; 
    }
}