using System;
using System.Collections.Generic;
using System.Text;

public class ChatRoom
{
    public event Action<string, string> MessageReceived;
    public void SendMessage(string sender, string message)
    {
        MessageReceived?.Invoke(sender, message);
    }
}